using System;
using GamesToGo.Game.Graphics;
using GamesToGo.Game.Online;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace GamesToGo.Game.Overlays
{
    public class SideMenuOverlay : OverlayContainer
    {
        private Container menu;
        private Box shadowBox;
        [Resolved]
        private APIController api { get; set; }
        private Bindable<User> localUser = new Bindable<User>();
        private SpriteText username;
        private Sprite userImage;
        private TextureStore textures;

        public Action NextScreen { get; set; }


        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            this.textures = textures;
            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                new Container
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        shadowBox = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Colour4.Black,
                            Alpha = 0,
                        },
                        new SurfaceButton
                        {
                            Action = Hide,
                        },
                    },
                },
                menu = new Container
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
                            Colour = new Colour4 (106,100,104, 255)
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
                                                    Padding = new MarginPadding(){ Top = 80, Left = 20, Right = 20},
                                                    Child = new SurfaceButton
                                                    {
                                                        //Action = profileScreen
                                                        Children = new Drawable[]
                                                        {
                                                            new EmptyBox
                                                            {
                                                                RelativeSizeAxes = Axes.Both
                                                            },
                                                            new FillFlowContainer
                                                            {
                                                                RelativeSizeAxes = Axes.Both,
                                                                Direction = FillDirection.Vertical,
                                                                Padding = new MarginPadding(){ Bottom = 40},
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
                                                                        Child = userImage= new Sprite
                                                                        {
                                                                            Anchor = Anchor.Centre,
                                                                            Origin = Anchor.Centre,
                                                                            RelativeSizeAxes = Axes.Both,
                                                                            Texture = textures.Get("Images/gtg")
                                                                        }
                                                                    },
                                                                    username = new SpriteText
                                                                    {
                                                                        Anchor = Anchor.Centre,
                                                                        Origin = Anchor.Centre,
                                                                        Text = "Guest",
                                                                        Font = new FontUsage(size: 80)
                                                                    },
                                                                }
                                                            },
                                                        }
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
                                                    Child = new SurfaceButton()
                                                    {
                                                        //Action
                                                        Children = new Drawable[]
                                                        {
                                                            new EmptyBox
                                                            {
                                                                RelativeSizeAxes = Axes.Both
                                                            },
                                                            new SpriteText
                                                            {
                                                                Anchor = Anchor.CentreRight,
                                                                Origin = Anchor.CentreRight,
                                                                Text = "Invitaciones",
                                                                Font = new FontUsage(size: 80)
                                                            },

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
                                                    Child = new SurfaceButton()
                                                    {
                                                        Action = logout,
                                                        Children = new Drawable[]
                                                        {
                                                            new EmptyBox
                                                            {
                                                                RelativeSizeAxes = Axes.Both
                                                            },
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
                                            new EmptyBox
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                            },
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
            };
            localUser.BindTo(api.LocalUser);
            localUser.BindValueChanged(_ => changeUserData());
        }

        private void changeUserData()
        {
            if(localUser.Value != null)
            {
                userImage.Texture = textures.Get($"https://gamestogo.company/api/Users/DownloadImage/{api.LocalUser.Value.ID}");
                username.Text = api.LocalUser.Value.Username + " #" + api.LocalUser.Value.ID;
            }
        }

        private void logout()
        {
            api.Logout();
            NextScreen?.Invoke();
            Hide();
        }

        protected override void PopIn()
        {
            shadowBox.FadeTo(0.75f, 700);
            menu.MoveToX(-1).Then().MoveToX(0, 700, Easing.OutElastic);
        }

        protected override void PopOut()
        {
            shadowBox.FadeOut(250, Easing.OutExpo);
            menu.MoveToX(-1,700, Easing.InElastic);
        }
    }
}

