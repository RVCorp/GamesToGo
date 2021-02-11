using System.Threading;
using System.Threading.Tasks;
using GamesToGo.Game.Graphics;
using GamesToGo.Game.Online;
using GamesToGo.Game.Online.Models.RequestModel;
using GamesToGo.Game.Online.Requests;
using GamesToGo.Game.Overlays;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Platform;
using osu.Framework.Screens;

namespace GamesToGo.Game.Screens
{
    public class RoomScreen : Screen
    {

        [Resolved]
        private APIController api { get; set; }
        [Resolved]
        private Storage store { get; set; }
        private OnlineRoom room;
        private FillFlowContainer<TextContainer> usersInRoom;
        private SpriteText textButton;
        private InviteOverlay inviteOverlay = new InviteOverlay();
        private CancellationTokenSource _tokenSource = null;
        private CancellationToken Token;
        private Box colorBox;

        public RoomScreen(OnlineRoom room)
        {
            this.room = room;
        }

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
                                        Anchor = Anchor.TopLeft,
                                        Origin = Anchor.TopLeft,
                                        RelativeSizeAxes = Axes.Both,
                                        Width = .2f,
                                        Child = new SimpleIconButton(FontAwesome.Solid.SignOutAlt)
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Action = () => exitRoom()
                                        },
                                    },
                                },
                            },
                        },
                        new Drawable[]
                        {
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Child = new FillFlowContainer
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Direction = FillDirection.Vertical,
                                    Padding = new MarginPadding(50),
                                    Children = new Drawable[]
                                    {
                                        new Container
                                        {
                                            RelativeSizeAxes = Axes.X,
                                            Height = 1000,
                                            Child = new FillFlowContainer
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                Direction = FillDirection.Vertical,
                                                Children = new Drawable[]
                                                {
                                                    new SpriteText
                                                    {
                                                        Text = "Jugadores:",
                                                        Font = new FontUsage(size: 80)
                                                    },
                                                    new BasicScrollContainer
                                                    {
                                                        RelativeSizeAxes = Axes.X,
                                                        Height = 1000,
                                                        ClampExtension = 30,
                                                        Masking = true,
                                                        Child = usersInRoom = new FillFlowContainer<TextContainer>
                                                        {
                                                            RelativeSizeAxes = Axes.Both,
                                                            Direction = FillDirection.Full,
                                                        },
                                                    },
                                                },
                                            },
                                        },
                                        new Container
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            Height = .15f,
                                            Child = new SurfaceButton
                                            {
                                                Action = inviteOverlay.Show,
                                                Children = new Drawable[]
                                                {
                                                    new Box
                                                    {
                                                        RelativeSizeAxes = Axes.Both,
                                                        Colour = Colour4.LightGray
                                                    },
                                                    textButton =new SpriteText
                                                    {
                                                        Anchor = Anchor.Centre,
                                                        Origin = Anchor.Centre,
                                                        Font = new FontUsage(size: 80),
                                                    }
                                                }
                                            }
                                        },
                                        new Container
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            Height = .15f,
                                            Child = new SurfaceButton
                                            {
                                                Action = playGame,
                                                Children = new Drawable[]
                                                {
                                                    colorBox = new Box
                                                    {
                                                        RelativeSizeAxes = Axes.Both,
                                                        Colour = Colour4.LightPink
                                                    },
                                                    textButton =new SpriteText
                                                    {
                                                        Anchor = Anchor.Centre,
                                                        Origin = Anchor.Centre,
                                                        Font = new FontUsage(size: 80),
                                                        Colour = new Colour4(106, 100, 104, 255)
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                inviteOverlay
            };
            if (api.LocalUser.Value.ID == room.Owner.BackingUser.ID)
                textButton.Text = "Jugar!";
            else
                textButton.Text = "Listo!";
            populateUsersList();
            _tokenSource = new CancellationTokenSource();
            var token = _tokenSource.Token;
            Token = token;
            Task.Run(() =>
            {
                while (!room.HasStarted)
                {
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                    System.Threading.Thread.Sleep(10000);
                    var roomStateRequest = new GetRoomStateRequest();
                    roomStateRequest.Success += u =>
                    {
                        if (u.HasStarted == true)
                        {
                            LoadComponentAsync(new GameScreen(room), this.Push);
                            _tokenSource.Cancel();
                        }
                        else
                            Refresh(u);
                    };
                    api.Queue(roomStateRequest);
                }
            });
        }

        private void exitRoom()
        {
            var req = new ExitRoomRequest();
            req.Success += () =>
            {
                this.Exit();
                _tokenSource.Cancel();
            };
            api.Queue(req);
        }

        private void playGame()
        {
            colorBox.Colour = Colour4.DeepPink;
            var req = new ReadyRequest();
            api.Queue(req);

        }

        private void populateUsersList()
        {
            for (int i = 0; i < room.Game.Maxplayers; i++)
            {
                    usersInRoom.Add(new TextContainer());
            }
            Refresh(room);
        }

        private void Refresh(OnlineRoom updatedRoom)
        {
            room = updatedRoom;
            for (int i = 0; i < updatedRoom.Game.Maxplayers; i++)
            {
                if(updatedRoom.Players[i] != null)
                {
                    usersInRoom[i].Text.Text = updatedRoom.Players[i].BackingUser.Username;
                }
                else if (updatedRoom.Players[i] == null)
                {
                    usersInRoom[i].Text.Text = "";
                }
            }
        }

        private class TextContainer : Container
        {
            public SpriteText Text;

            [BackgroundDependencyLoader]
            private void load()
            {
                RelativeSizeAxes = Axes.X;
                Width = .5f;
                Height = 100;
                Child = Text = new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Font = new FontUsage(size: 50)
                };
            }
        }
    }
}
