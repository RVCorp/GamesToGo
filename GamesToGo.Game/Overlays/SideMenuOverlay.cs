using GamesToGo.Common.Online;
using GamesToGo.Common.Online.RequestModel;
using GamesToGo.Game.Graphics;
using GamesToGo.Game.Online;
using GamesToGo.Game.Online.Models.RequestModel;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using osuTK;

namespace GamesToGo.Game.Overlays
{
    [Cached]
    public class SideMenuOverlay : OverlayContainer
    {
        [Resolved]
        private APIController api { get; set; }

        [Resolved]
        private GamesToGoGame game { get; set; }

        [Resolved]
        private ScreenStack stack { get; set; }

        [Resolved]
        private TextureStore textures { get; set; }

        [Cached]
        private ProfileOverlay profileOverlay = new ProfileOverlay();

        [Cached]
        private InvitationsOverlay invitationsOverlay = new InvitationsOverlay();

        private readonly Bindable<User> localUser = new Bindable<User>();
        private readonly BindableList<Invitation> invitations = new BindableList<Invitation>();
        private SpriteText username;
        private Sprite userImage;
        private SpriteText invitationsNumber;
        private Container numberContainer;

        protected override bool StartHidden => true;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativePositionAxes = Axes.X;
            RelativeSizeAxes = Axes.Both;
            X = -1;

            InternalChildren = new Drawable[]
            {
                new InvitationsUpdater(),
                new SurfaceButton
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    BackgroundColour = ColourInfo.GradientHorizontal(Colour4.Black.Opacity(0.6f).ToLinear(), Colour4.Black.Opacity(0f).ToLinear()),
                    Width = 0.2f,
                    Action = Hide,
                },
                new Box
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopRight,
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Black.Opacity(0.6f).ToLinear(),
                },
                new Container
                {
                    RelativePositionAxes = Axes.Both,
                    RelativeSizeAxes = Axes.Both,
                    Width = .8f,
                    BorderColour = Colour4.Black,
                    BorderThickness = 3.5f,
                    Masking = true,
                    Children = new Drawable[]
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
                                new Dimension(GridSizeMode.Relative, .9f),
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
                                        Child = new FillFlowContainer
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            Direction = FillDirection.Vertical,
                                            Children = new Drawable[]
                                            {
                                                new Container
                                                {
                                                    RelativeSizeAxes = Axes.Both,
                                                    Height = .3f,
                                                    BorderColour = Colour4.Black,
                                                    BorderThickness = 3.5f,
                                                    Masking = true,
                                                    Padding = new MarginPadding {Top = 80, Left = 20, Right = 20},
                                                    Child = new SurfaceButton
                                                    {
                                                        Action = profileScreen,
                                                        Children = new Drawable[]
                                                        {
                                                            new FillFlowContainer
                                                            {
                                                                RelativeSizeAxes = Axes.Both,
                                                                Direction = FillDirection.Vertical,
                                                                Padding = new MarginPadding() {Bottom = 40},
                                                                Children = new Drawable[]
                                                                {
                                                                    new CircularContainer
                                                                    {
                                                                        Anchor = Anchor.Centre,
                                                                        Origin = Anchor.Centre,
                                                                        BorderColour = Colour4.Black,
                                                                        BorderThickness = 3.5f,
                                                                        Masking = true,
                                                                        Size = new Vector2(300, 300),
                                                                        Child = userImage = new Sprite
                                                                        {
                                                                            Anchor = Anchor.Centre,
                                                                            Origin = Anchor.Centre,
                                                                            RelativeSizeAxes = Axes.Both,
                                                                            Texture = textures.Get("Images/gtg"),
                                                                        },
                                                                    },
                                                                    username = new SpriteText
                                                                    {
                                                                        Anchor = Anchor.Centre,
                                                                        Origin = Anchor.Centre,
                                                                        Text = "Guest",
                                                                        Font = new FontUsage(size: 80)
                                                                    },
                                                                },
                                                            },
                                                        },
                                                    },
                                                },
                                                new Container
                                                {
                                                    RelativeSizeAxes = Axes.Both,
                                                    Height = .15f,
                                                    BorderColour = Colour4.Black,
                                                    BorderThickness = 3.5f,
                                                    Masking = true,
                                                    Padding = new MarginPadding(20),
                                                    Child = new SurfaceButton
                                                    {
                                                        Action = () => invitesScreen(),
                                                        Children = new Drawable[]
                                                        {
                                                            new FillFlowContainer
                                                            {
                                                                RelativeSizeAxes = Axes.Both,
                                                                Spacing = new Vector2(30),
                                                                Children = new Drawable[]
                                                                {
                                                                    numberContainer = new Container
                                                                    {
                                                                        Anchor = Anchor.CentreRight,
                                                                        Origin = Anchor.CentreRight,

                                                                        Children = new Drawable[]
                                                                        {
                                                                            new Box
                                                                            {
                                                                                RelativeSizeAxes = Axes.Both,
                                                                                Colour = Colour4.LightBlue,
                                                                            },
                                                                            invitationsNumber = new SpriteText
                                                                            {
                                                                                Anchor = Anchor.Centre,
                                                                                Origin = Anchor.Centre,
                                                                                Font = new FontUsage(size: 70)
                                                                            },

                                                                        }
                                                                    },
                                                                    new SpriteText
                                                                    {
                                                                        Anchor = Anchor.CentreRight,
                                                                        Origin = Anchor.CentreRight,
                                                                        Text = "Invitaciones",
                                                                        Font = new FontUsage(size: 80)
                                                                    },
                                                                }
                                                            }

                                                        },
                                                    }
                                                },
                                                new Container
                                                {
                                                    RelativeSizeAxes = Axes.Both,
                                                    Height = .15f,
                                                    BorderColour = Colour4.Black,
                                                    BorderThickness = 3.5f,
                                                    Masking = true,
                                                    Padding = new MarginPadding(20),
                                                    Child = new SurfaceButton
                                                    {
                                                        Action = logout,
                                                        Children = new Drawable[]
                                                        {
                                                            new SpriteText
                                                            {
                                                                Anchor = Anchor.CentreRight,
                                                                Origin = Anchor.CentreRight,
                                                                Text = "Cerrar Sesión",
                                                                Font = new FontUsage(size: 80)
                                                            },

                                                        },
                                                    }
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
                                        BorderColour = Colour4.Black,
                                        BorderThickness = 3.5f,
                                        Masking = true,
                                        Padding = new MarginPadding(20),
                                        Children = new Drawable[]
                                        {
                                            new SpriteText
                                            {
                                                Anchor = Anchor.CentreRight,
                                                Origin = Anchor.CentreRight,
                                                Text = "GamesToGo",
                                                Font = new FontUsage(size: 80)
                                            },
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
                profileOverlay,
                invitationsOverlay
            };
            invitations.BindTo(game.Invitations);
            invitations.BindCollectionChanged((_,__)=> changeInvitationsNumber());
            localUser.BindTo(api.LocalUser);
            localUser.BindValueChanged(_ => changeUserData(), true);
        }

        private void logout()
        {
            Hide();
            game.Logout();
        }

        private void changeInvitationsNumber()
        {
            if(invitations.Count != 0)
            {
                invitationsNumber.Text = invitations.Count.ToString();
                numberContainer.Height = 100;
                numberContainer.Width = 150;
            }
            else
            {
                invitationsNumber.Text = "";
                numberContainer.Height = 0;
                numberContainer.Width = 0;
            }
        }

        private void changeUserData()
        {
            if(localUser.Value != null)
            {
                Schedule(async () =>



                {
                    userImage.Texture = await textures.GetAsync(@$"https://gamestogo.company/api/Users/DownloadImage/{api.LocalUser.Value.ID}");
                });
                username.Text = api.LocalUser.Value.Username + " #" + api.LocalUser.Value.ID;
            }
        }

        protected override void PopIn()
        {
            this.MoveToX(0, 300, Easing.OutBack);
        }

        protected override void PopOut()
        {
            this.MoveToX(-1,300, Easing.InBack);
        }

        private void invitesScreen()
        {
            invitationsOverlay.Show();
        }

        private void profileScreen()
        {
            profileOverlay.Show();
        }
    }
}

