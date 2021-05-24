using GamesToGo.Common.Graphics;
using GamesToGo.Common.Online;
using GamesToGo.Common.Online.Requests;
using GamesToGo.Editor.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace GamesToGo.Editor.Screens
{
    public class ProfileScreen : Screen
    {
        [Resolved]
        private APIController api { get; set; }

        [Resolved]
        private TextureStore textures { get; set; }

        private FillFlowContainer<PublishedProjectSummaryContainer> publishedProjectsList;
        private FillFlowContainer<Container> statisticsContainer;

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Color4 (106, 100, 104, 255),      //Color fondo general
                },
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            RelativeSizeAxes = Axes.X,
                            Height = 35,

                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = Color4.DarkGray,
                                },
                                new GamesToGoButton
                                {
                                    RelativeSizeAxes = Axes.Y,
                                    Width = 100,
                                    Text = @"Regresar",
                                    Action = this.Exit,
                                },
                            },
                        },
                        new BasicScrollContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Child = new FillFlowContainer
                            {
                                RelativeSizeAxes  = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Direction = FillDirection.Vertical,
                                Children = new Drawable[]
                                {
                                    new Container
                                    {
                                        RelativeSizeAxes = Axes.X,
                                        Height = 600,
                                        Children = new Drawable[]
                                        {
                                            new Sprite
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                Texture = textures.Get("https://a-static.besthdwallpaper.com/halo-infinito-papel-pintado-1920x600-11770_57.jpg")
                                            },
                                            new FillFlowContainer
                                            {
                                                Anchor = Anchor.Centre,
                                                Origin = Anchor.Centre,
                                                RelativeSizeAxes = Axes.Y,
                                                Width = 400,
                                                Direction = FillDirection.Vertical,
                                                Padding = new MarginPadding(30),
                                                Spacing = new Vector2(50),
                                                Children = new Drawable[]
                                                {
                                                    new UserImageChangerButton
                                                    {
                                                        Anchor = Anchor.TopCentre,
                                                        Origin = Anchor.TopCentre,
                                                        ButtonSize = new Vector2(400),
                                                    },
                                                    new SpriteText
                                                    {
                                                        Text = api.LocalUser.Value.Username,
                                                        Anchor = Anchor.TopCentre,
                                                        Origin = Anchor.TopCentre,
                                                        Font =  new FontUsage(size:60),
                                                    },
                                                },
                                            },
                                        },
                                    },
                                    new FillFlowContainer
                                    {
                                        RelativeSizeAxes = Axes.X,
                                        Height = 600,
                                        Direction = FillDirection.Horizontal,
                                        Children = new Drawable[]
                                        {
                                            new Container
                                            {
                                                RelativeSizeAxes = Axes.Y,
                                                Width = 1500,
                                                Child = new FillFlowContainer
                                                {
                                                    RelativeSizeAxes = Axes.Both,
                                                    Direction = FillDirection.Vertical,
                                                    Children = new Drawable[]
                                                    {
                                                        new Container
                                                        {
                                                            RelativeSizeAxes = Axes.X,
                                                            Height = 35,
                                                            Children = new Drawable[]
                                                            {
                                                                new Box
                                                                {
                                                                    RelativeSizeAxes = Axes.Both,
                                                                    Colour = Color4.DarkGray,
                                                                    Margin = new MarginPadding { Right = 2.5f },
                                                                },
                                                                new SpriteText
                                                                {
                                                                    Text = @"Juegos Publicados",
                                                                    Font = new FontUsage(size: 35),
                                                                    Padding = new MarginPadding { Left = 5 },
                                                                },
                                                            },
                                                        },
                                                        publishedProjectsList = new FillFlowContainer<PublishedProjectSummaryContainer>
                                                        {
                                                            BorderColour = Color4.Black,
                                                            BorderThickness = 3f,
                                                            Masking = true,
                                                            Anchor = Anchor.TopCentre,
                                                            Origin = Anchor.TopCentre,
                                                            Spacing = Vector2.Zero,
                                                            RelativeSizeAxes = Axes.X,
                                                            AutoSizeAxes = Axes.Y,
                                                            Direction = FillDirection.Vertical,

                                                        },
                                                    },
                                                },
                                            },
                                            new Container
                                            {
                                                RelativeSizeAxes = Axes.Y,
                                                Width = 480,
                                                Child = new FillFlowContainer
                                                {
                                                    RelativeSizeAxes = Axes.Both,
                                                    Direction = FillDirection.Vertical,
                                                    Children = new Drawable[]
                                                    {
                                                        new Container
                                                        {
                                                            RelativeSizeAxes = Axes.X,
                                                            Height = 35,
                                                            Children = new Drawable[]
                                                            {
                                                                new Box
                                                                {
                                                                    RelativeSizeAxes = Axes.Both,
                                                                    Colour = Color4.DarkGray,
                                                                    Margin = new MarginPadding{ Left = 2.5f },
                                                                },
                                                                new SpriteText
                                                                {
                                                                    Text = @"Estadisticas",
                                                                    Font = new FontUsage(size: 35),
                                                                    Padding = new MarginPadding { Left = 5 },
                                                                },
                                                            },
                                                        },
                                                        new Container
                                                        {
                                                            RelativeSizeAxes = Axes.Both,
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
                                                    },
                                                },
                                            },
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
            };

            var stats = new GetUserStatisticsRequest();
            stats.Success += (u) =>
            {
                foreach (var stat in u)
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

        public override void OnEntering(IScreen last)
        {
            base.OnResuming(last);

            publishedProjectsList.Clear();
            populateOnlineList();
        }

        private void populateOnlineList()
        {
            var getProjects = new GetAllUserPublishedGamesRequest();
            getProjects.Success += u =>
            {
                foreach (var proj in u)
                {
                    publishedProjectsList.Add(new PublishedProjectSummaryContainer(proj));
                }
            };
            api.Queue(getProjects);
        }
    }
}
