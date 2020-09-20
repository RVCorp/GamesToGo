using System;
using System.Collections.Generic;
using System.Linq;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Project.Elements;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GamesToGo.Desktop.Graphics
{
    public class ObjectListingContainer<TElement, TButton> : Container
        where TElement : ProjectElement, new()
        where TButton : ElementEditButton, new()
    {
        private IBindableList<ProjectElement> localElements = new BindableList<ProjectElement>();

        private FillFlowContainer<TButton> allElements;

        private Action receivedAction = null;

        public Action EditAction
        {
            set
            {
                if (allElements != null)
                    foreach (var element in allElements)
                        element.Action = value;
                receivedAction = value;
            }
        }

        private Func<TElement, bool> filter = (_) => true;
        public Func<TElement, bool> Filter
        {
            get => filter;
            set
            {
                filter = value;
                allElements.Clear();
                checkAdded(localElements);
            }
        }

        public Colour4 BackgroundColour
        {
            get
            {
                if (typeof(TElement).Equals(typeof(Card)))
                    return Colour4.Maroon;
                if (typeof(TElement).Equals(typeof(Token)))
                    return Colour4.Crimson;
                if (typeof(TElement).Equals(typeof(Board)))
                    return Colour4.DarkSeaGreen;
                if (typeof(TElement).Equals(typeof(Tile)))
                    return Colour4.BlueViolet;
                return Colour4.Black;
            }
        }

        public string Title
        {
            get
            {
                if (typeof(TElement).Equals(typeof(Card)))
                    return "Cartas";
                if (typeof(TElement).Equals(typeof(Token)))
                    return "Fichas";
                if (typeof(TElement).Equals(typeof(Board)))
                    return "Tableros";
                if (typeof(TElement).Equals(typeof(Tile)))
                    return "Casillas";
                return "Elementos raros";
            }
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Add(new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new Dimension[]
                {
                    new Dimension(GridSizeMode.AutoSize),
                    new Dimension(),
                },
                Content = new Drawable[][]
                {
                    new Drawable[]
                    {
                        new Container
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    RelativeSizeAxes = Axes.X,
                                    Colour = BackgroundColour,
                                    Height = 50,
                                },
                                new SpriteText
                                {
                                    Text = Title,
                                    Font = new FontUsage(size: 45),
                                    Position = new Vector2(5, 2.5f),
                                    Shadow = true,
                                },
                            }
                        },
                    },
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
                                    Colour = BackgroundColour.Opacity(0.3f),
                                },
                                new ObjectManagerScrollContainer
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    ClampExtension = 30,
                                    RelativeSizeAxes = Axes.Both,
                                    Child = allElements = new FillFlowContainer<TButton>
                                    {
                                        RelativeSizeAxes = Axes.X,
                                        AutoSizeAxes = Axes.Y,
                                        Direction = FillDirection.Full,
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        Padding = new MarginPadding { Vertical = 10 },
                                        Spacing = new Vector2(5),
                                    },
                                },
                            }
                        }

                    },
                }
            });
        }

        private void checkAdded(IEnumerable<ProjectElement> added)
        {
            var resultingAdded = new List<TElement>();
            foreach (var item in added)
            {
                if (item is TElement itemT)
                {
                    if (Filter(itemT))
                    {
                        resultingAdded.Add(itemT);
                        allElements.Add(new TButton() { Element = itemT, Action = receivedAction });
                    }
                }
            }
        }

        private void checkRemoved(IEnumerable<ProjectElement> removed)
        {
            var resultingRemoved = new List<TElement>();
            foreach (var item in removed)
            {
                if (item is TElement itemT && Filter(itemT))
                {
                    var deletable = allElements.FirstOrDefault(b => b.Element.ID == item.ID);

                    if (deletable != null)
                    {
                        resultingRemoved.Add(itemT);
                        allElements.Remove(deletable);
                    }
                }
            }
        }

        public void BindToList(IBindableList<ProjectElement> elements)
        {
            localElements.UnbindAll();

            allElements.Clear();
            checkAdded(elements);

            localElements.BindTo(elements);

            localElements.ItemsAdded += checkAdded;
            localElements.ItemsRemoved += checkRemoved;
        }

        private class ObjectManagerScrollContainer : BasicScrollContainer
        {
            public ObjectManagerScrollContainer()
            {
                Scrollbar.Blending = BlendingParameters.Additive;
                Scrollbar.Child.Colour = new Colour4(50, 50, 50, 255);
            }
        }
    }
}
