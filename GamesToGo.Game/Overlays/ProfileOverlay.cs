using System;
using GamesToGo.Common.Online;
using GamesToGo.Common.Online.Requests;
using GamesToGo.Game.Graphics;
using GamesToGo.Game.Online.Requests;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace GamesToGo.Game.Overlays
{
    public class ProfileOverlay : OverlayContainer
    {
        private FillFlowContainer<Container> publishedGames;
        private FillFlowContainer<Container> statisticsContainer;
        private Sprite userImage;
        [Resolved]
        private TextureStore textures { get; set; }
        private SpriteText userInfo;
        [Resolved]
        private SideMenuOverlay sideMenu { get; set; }

        [Resolved]
        private APIController api { get; set; }
        public Action NextScreen { get; set; }

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
                    RowDimensions = new []
                    {
                        new Dimension(GridSizeMode.Relative, .1f),
                        new Dimension(GridSizeMode.Relative, .4f),
                        new Dimension(),
                    },
                    ColumnDimensions = new []
                    {
                        new Dimension()
                    },
                    Content = new []
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
                                Child = new FillFlowContainer
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Direction = FillDirection.Horizontal,
                                    Children =  new Drawable[]
                                    {
                                        new Container
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            Width = .55f,
                                            Child = new FillFlowContainer
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                Direction = FillDirection.Vertical,
                                                Padding = new MarginPadding(30),
                                                Children = new Drawable[]
                                                {
                                                    new CircularContainer
                                                    {
                                                        Anchor = Anchor.Centre,
                                                        Origin = Anchor.Centre,
                                                        BorderColour = Colour4.Black,
                                                        BorderThickness = 3.5f,
                                                        Masking = true,
                                                        Size = new Vector2(450, 450),
                                                        Child = userImage = new Sprite
                                                        {
                                                            Anchor = Anchor.Centre,
                                                            Origin = Anchor.Centre,
                                                            RelativeSizeAxes = Axes.Both,
                                                            Texture = textures.Get("Images/gtg")
                                                        }
                                                    },
                                                    userInfo = new SpriteText
                                                    {
                                                        Anchor = Anchor.Centre,
                                                        Origin = Anchor.Centre,
                                                        Text = "Generic User xdxdxd",
                                                        Font = new FontUsage(size: 60)
                                                    },
                                                }
                                            }
                                        },
                                        new Container
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            Width = .45f,
                                            Child = new BasicScrollContainer
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                ClampExtension = 30,
                                                Child = statisticsContainer = new FillFlowContainer<Container>
                                                {
                                                    AutoSizeAxes = Axes.Y,
                                                    RelativeSizeAxes = Axes.X,
                                                    Direction = FillDirection.Vertical,
                                                    Padding = new MarginPadding(40)
                                                },
                                            }
                                        }
                                    }
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
                                    Child = publishedGames = new FillFlowContainer<Container>
                                    {
                                        AutoSizeAxes = Axes.Y,
                                        RelativeSizeAxes = Axes.X,
                                        Direction = FillDirection.Vertical,
                                    },
                                }
                            }
                        }
                    }
                }
            };
        }

        private void populateContainers()
        {
            publishedGames.Clear();
            statisticsContainer.Clear();
            var games = new GetAllUserPublishedGamesRequest();
            games.Success += (u) =>
            {
                foreach(var game in u)
                {
                    publishedGames.Add(new Container
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 400,
                        Children = new Drawable[]
                        {
                            new GamePreviewContainer(game)
                            {
                                GameNameSize = 90,
                                MadeBySize = 60
                            }
                        },
                    });
                }
            };
            api.Queue(games);

            var stats = new GetUserStatisticsRequest();
            stats.Success += (u) =>
            {
                foreach(var stat in u)
                {
                    statisticsContainer.Add(new Container
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 300,
                        Child = new StatisticContainer(stat)
                    });
                }
            };
            api.Queue(stats);

        }

        protected override void PopIn()
        {
            populateContainers();
            Schedule(async () =>
            {
                userImage.Texture = await textures.GetAsync(@$"https://gamestogo.company/api/Users/DownloadImage/{api.LocalUser.Value.ID}");
            });
            userInfo.Text = api.LocalUser.Value.Username + "  #" + api.LocalUser.Value.ID;
            this.FadeIn(300);
        }

        protected override void PopOut()
        {
            this.FadeOut(300);
        }
    }
}
