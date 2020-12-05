using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GamesToGo.Game.Graphics;
using GamesToGo.Game.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
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
        private TagFlowContainer tagsContainer;
        private List<Bindable<bool>> tagsAreSelected;

        [Resolved]
        private MainMenuScreen mainMenu { get; set; }

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
                                                            Child = new BasicScrollContainer(Direction.Horizontal)
                                                            {
                                                                RelativeSizeAxes = Axes.X,
                                                                Height = 150,
                                                                ClampExtension = 30,
                                                                Child = tagsContainer = new TagFlowContainer
                                                                {
                                                                    AutoSizeAxes = Axes.X,
                                                                    RelativeSizeAxes = Axes.Y,
                                                                    Direction = FillDirection.Horizontal,
                                                                },
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
            populateTags();
            tagsContainer.Current.BindValueChanged(v => enableButton());
            searchTextBox.Current.BindValueChanged(t => enableButton());
            search.SpriteText.Font = new FontUsage(size: 60);
            search.Enabled.Value = false;
        }

        private void populateTags()
        {
            for (int i = 0; i < 13; i++)
            {
                tagsContainer.Add(new TagContainer($"Etiqueta #{i}", 1) { CornerRadius = 10, Masking = true });
            }
        }

        private void enableButton()
        {
            if (tagsContainer.Current.Value > 0 || string.IsNullOrEmpty(searchTextBox.Current.Value) == false)
                search.Enabled.Value = true;
            else
                search.Enabled.Value = false;
        }

        private void searchRequest()
        {

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
