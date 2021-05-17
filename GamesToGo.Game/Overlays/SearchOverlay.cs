using GamesToGo.Common.Graphics;
using GamesToGo.Game.Graphics;
using GamesToGo.Game.Online.Requests;
using GamesToGo.Game.Screens;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;


namespace GamesToGo.Game.Overlays
{
    [Cached]
    public class SearchOverlay : OverlayContainer
    {
        private Box shadowBox;
        private Container content;
        private GamesToGoButton search;
        private BasicTextBox searchTextBox;
        private TagSelectionContainer tagsContainer;

        [Resolved]
        private SearchScreen searchScreen { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                shadowBox = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Black,
                    Alpha = 0,
                },
                content = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new GridContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            RowDimensions = new[]
                            {
                                new Dimension(GridSizeMode.Relative, .1f),
                                new Dimension(GridSizeMode.Relative, .6f),
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
                                        Padding = new MarginPadding(10),
                                        Child = new SimpleIconButton(FontAwesome.Solid.Times)
                                        {
                                            Anchor = Anchor.TopRight,
                                            Origin = Anchor.TopRight,
                                            Action = Hide,
                                        },
                                    },
                                },
                                new Drawable[]
                                {
                                    new Container
                                    {
                                        Depth = 0,
                                        RelativeSizeAxes = Axes.Both,
                                        Children = new Drawable[]
                                        {
                                            new FillFlowContainer
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                Direction = FillDirection.Vertical,
                                                Padding = new MarginPadding(30),
                                                Children = new Drawable[]
                                                {
                                                    new Container
                                                    {
                                                        Anchor = Anchor.TopLeft,
                                                        Origin = Anchor.TopLeft,
                                                        RelativeSizeAxes = Axes.X,
                                                        Height = 200,
                                                        Child = searchTextBox = new BasicTextBox
                                                        {
                                                            Anchor = Anchor.Centre,
                                                            Origin = Anchor.Centre,
                                                            RelativeSizeAxes = Axes.X,
                                                            Width = .9f,
                                                            Height = 150,
                                                        }
                                                    },
                                                    new Container
                                                    {
                                                        Anchor = Anchor.TopLeft,
                                                        Origin = Anchor.TopLeft,
                                                        RelativeSizeAxes = Axes.X,
                                                        Height = 150,
                                                        Child = new Container
                                                        {
                                                            Anchor = Anchor.Centre,
                                                            Origin = Anchor.Centre,
                                                            RelativeSizeAxes = Axes.Both,
                                                            Width = .9f,
                                                            Child = tagsContainer = new TagSelectionContainer(100)
                                                            {                                                                
                                                                RelativeSizeAxes = Axes.X,
                                                                Height = 100
                                                            }
                                                        }
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
                                        Depth = 1,
                                        RelativeSizeAxes = Axes.Both,
                                        Padding = new MarginPadding(30),
                                        Child = search = new GamesToGoButton
                                        {                                            
                                            Anchor = Anchor.TopCentre,
                                            Origin = Anchor.TopCentre,
                                            Height = 225,
                                            RelativeSizeAxes = Axes.X,
                                            Text = @"Buscar",
                                            Action = () => searchRequest()
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
            };
            tagsContainer.Current.BindValueChanged(v => enableButton());
            searchTextBox.Current.BindValueChanged(t => enableButton());
            search.SpriteText.Font = new FontUsage(size: 60);
            search.Enabled.Value = false;
        }

        private void enableButton()
        {
            if (tagsContainer.Current.Value > 0)
            {
                search.Enabled.Value = true;
                searchTextBox.Current.Disabled = true;
            }                
            else if(string.IsNullOrEmpty(searchTextBox.Current.Value) == false)
            {
                tagsContainer.Current.Disabled = true;
                search.Enabled.Value = true;
            }
            else
            {
                searchTextBox.Current.Disabled = false;
                search.Enabled.Value = false;
                tagsContainer.Current.Disabled = false;
            }                
        }

        private void searchRequest()
        {
            if (tagsContainer.Current.Value > 0)
            {
                var searchTag = new SearchTagRequest((uint)tagsContainer.Current.Value);
                searchTag.Success += u =>
                {
                    searchScreen.RefreshSearchedGames(u);
                };
            }
            else
            {
                var searchString = new SearchTextRequest(searchTextBox.Current.Value);
                searchString.Success += u =>
                {
                    searchScreen.RefreshSearchedGames(u);
                };
            }
        }

        protected override void PopIn()
        {            
            shadowBox.FadeTo(0.9f, 250);
            content.FadeIn(250);
        }

        protected override void PopOut()
        {
            shadowBox.FadeOut(250, Easing.OutExpo);
            content.FadeOut(250);
        }
    }
}
