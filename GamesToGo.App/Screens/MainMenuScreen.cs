using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GamesToGo.App.Graphics;
using GamesToGo.App.Online;
using GamesToGo.App.Overlays;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;

namespace GamesToGo.App.Screens
{
    [Cached]
    public class MainMenuScreen : Screen
    {
        private FillFlowContainer<Container> comunityGames;
        [Cached]
        private DropdownMenuOverlay dropdownMenu;
        [Resolved]
        private APIController api { get; set; }
        public MainMenuScreen()
        {
            dropdownMenu = new DropdownMenuOverlay();
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
                                        Child = new SimpleIconButton(FontAwesome.Solid.Bars)
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Action = () => dropdownMenu.Show()
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
                                    Child = comunityGames = new FillFlowContainer<Container>
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
                dropdownMenu
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
                    comunityGames.Add(new Container
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 400,
                        Children = new Drawable[]
                        {
                            new GamePreviewContainer(game),
                            new SurfaceButton {Action = () => gameScreen(game) }
                        }
                    });
                }
            };
            api.Queue(getGames);
        }
        
        private bool gameScreen(OnlineGame game)
        {
            LoadComponentAsync(new GameInfoScreen(game), this.Push);
            return true;
        }

        
    }
}
