using GamesToGo.Game.Graphics;
using GamesToGo.Game.Online;
using GamesToGo.Game.Overlays;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;

namespace GamesToGo.Game.Screens
{
    [Cached]
    public class MainMenuScreen : Screen
    {
        private FillFlowContainer<Container> communityGames;
        [Cached]
        private SideMenuOverlay sideMenu = new SideMenuOverlay();
        [Resolved]
        private APIController api { get; set; }

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
                                        Child = new SimpleIconButton(FontAwesome.Solid.Bars)
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Action = () => sideMenu.Show()
                                        }
                                    },
                                    new Container
                                    {
                                        Anchor = Anchor.TopRight,
                                        Origin = Anchor.TopRight,
                                        RelativeSizeAxes = Axes.Both,
                                        Width = .2f,
                                        Child = new SimpleIconButton(FontAwesome.Solid.Search)
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre
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
                                    Child = communityGames = new FillFlowContainer<Container>
                                    {
                                        AutoSizeAxes = Axes.Y,
                                        RelativeSizeAxes = Axes.X,
                                        Direction = FillDirection.Vertical,
                                    },
                                }
                            }
                        }
                    }
                },
                sideMenu
            };
            populateGamesList();
        }

        private void populateGamesList()
        {
            var getGames = new GetAllPublishedGamesRequest();
            getGames.Success += u =>
            {
                foreach(var game in u)
                {
                    communityGames.Add(new Container
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 400,
                        Children = new Drawable[]
                        {
                            new GamePreviewContainer(game)
                            {
                                Action = () => LoadComponentAsync(new GameInfoScreen(game), this.Push)
                            },
                        },
                    });
                }
            };
            api.Queue(getGames);
        }
    }
}
