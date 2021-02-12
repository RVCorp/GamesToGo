using System.Linq;
using System.Threading.Tasks;
using GamesToGo.Common.Online;
using GamesToGo.Common.Online.RequestModel;
using GamesToGo.Common.Overlays;
using GamesToGo.Game.Graphics;
using GamesToGo.Game.Online.Requests;
using GamesToGo.Game.Screens;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;

namespace GamesToGo.Game.Overlays
{
    public class InvitationsOverlay : OverlayContainer
    {
        private FillFlowContainer<InvitePreviewContainer> invitationsContainer;

        [Resolved]
        private GamesToGoGame game { get; set; }
        [Resolved]
        private APIController api { get; set; }
        [Resolved]
        private ScreenStack stack { get; set; }
        [Resolved]
        private SideMenuOverlay sideMenu { get; set; }
        [Resolved]
        private SplashInfoOverlay infoOverlay { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Colour4(106, 100, 104, 255)
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    RowDimensions = new[]
                    {
                        new Dimension(GridSizeMode.Relative, .1f),
                        new Dimension()
                    },
                    ColumnDimensions = new[]
                    {
                        new Dimension()
                    },
                    Content = new[]
                    {
                        new Drawable[]
                        {
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = new Colour4(60, 60, 60, 255)
                                    },
                                    new Container
                                    {
                                        Anchor = Anchor.TopRight,
                                        Origin = Anchor.TopRight,
                                        RelativeSizeAxes = Axes.Both,
                                        Width = .2f,
                                        Child = new SimpleIconButton(FontAwesome.Solid.Home)
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Action = () =>
                                            {
                                                Hide();
                                                sideMenu.Hide();
                                            }
                                        }
                                    },
                                }
                            }
                        },
                        new Drawable[]
                        {
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Child = new BasicScrollContainer
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    ClampExtension = 30,
                                    Child = invitationsContainer = new FillFlowContainer<InvitePreviewContainer>
                                    {
                                        AutoSizeAxes = Axes.Y,
                                        RelativeSizeAxes = Axes.X,
                                        Direction = FillDirection.Vertical,
                                    },
                                },
                            },
                        },
                    },
                },
            };
        }

        private void populateInvites()
        {
            invitationsContainer.Clear();
            foreach(var invite in game.Invitations)
            {
                invitationsContainer.Add(new InvitePreviewContainer(invite)
                {
                    NextScreen = () =>
                    {
                        joinRoom(invite.ID, invite.Room.Game);
                    },
                    DeleteInvitation = () => declineInvite(invite.ID)
                });
            }
        }

        private void joinRoom(int id, OnlineGame downloadGame)
        {
            var downloadComplete = game.DownloadGame(downloadGame);

            Task.Run(async () =>
            {
                await downloadComplete.Task;

                if (downloadComplete.Task.Result == false)
                {
                    failure();

                    return;
                }

                var room = new AcceptInviteRequest(id);
                room.Success += u =>
                {
                    Schedule(() =>
                    {
                        LoadComponentAsync(new RoomScreen(u), roomScreen =>
                        {
                            stack.Push(roomScreen);
                            Hide();
                            sideMenu.Hide();
                        });
                        game.Invitations.Remove(game.Invitations.First(i => i.ID == id));
                        invitationsContainer.Remove(invitationsContainer.FirstOrDefault(c => c.Invitation.ID == id));
                    });
                };
                room.Failure += ex =>
                {
                    failure();
                };

                api.Queue(room);

                void failure()
                {
                    Schedule(() => infoOverlay.Show(@"Ocurrió un error al intentar crear la sala", Colour4.DarkRed));
                }
            });
        }

        private void declineInvite(int id) // (id invitacion) POST
        {
            var invitation = new IgnoreInvitationRequest(id);
            invitation.Success += () =>
            {
                invitationsContainer.Remove(invitationsContainer.FirstOrDefault(c => c.Invitation.ID == id));
            };
            api.Queue(invitation);
        }

        protected override void PopIn()
        {
            populateInvites();
            this.FadeIn(300);
        }

        protected override void PopOut()
        {
            this.FadeOut(300);
        }
    }
}
