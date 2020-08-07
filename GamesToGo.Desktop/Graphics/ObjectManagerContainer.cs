using System;
using System.Collections.Generic;
using System.Linq;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Project.Elements;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Graphics
{
    public class ObjectManagerContainer<TElement, TButton> : Container
        where TElement : ProjectElement, new()
        where TButton : ElementEditButton, new()
    {
        private IBindableList<ProjectElement> localElements = new BindableList<ProjectElement>();

        private FillFlowContainer<TButton> allElements;
        private AutoSizeButton addElementButton;
        private readonly string title;
        private readonly string buttonText;

        protected Action ButtonAction { set => addElementButton.Action = value; }

        private Action editAction;

        protected Action EditAction
        {
            get => editAction;
            set
            {
                editAction = value;
                foreach(var button in allElements)
                {
                    button.Action = value;
                }
            }
        }

        public Func<TElement, bool> Filter { get; set; } = (_) => true;

        public Action<IEnumerable<TElement>> ItemsAdded { get; set; }
        public Action<IEnumerable<TElement>> ItemsRemoved { get; set; }

        public Color4 BackgroundColour
        {
            get
            {
                if (typeof(TElement).Equals(typeof(Card)))
                    return Color4.Maroon;
                if (typeof(TElement).Equals(typeof(Token)))
                    return Color4.Crimson;
                if (typeof(TElement).Equals(typeof(Board)))
                    return Color4.DarkSeaGreen;
                if (typeof(TElement).Equals(typeof(Tile)))
                    return Color4.BlueViolet;
                return Color4.Black;
            }
        }

        public ObjectManagerContainer(string title, string buttonText = null)
        {
            this.title = title;
            this.buttonText = buttonText ?? "Añadir nuevo";
        }

        [BackgroundDependencyLoader]
        private void load(WorkingProject project)
        {
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    RowDimensions = new Dimension[]
                    {
                        new Dimension(GridSizeMode.AutoSize),
                        new Dimension(),
                        new Dimension(GridSizeMode.AutoSize),
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
                                        Text = title,
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
                        new Drawable[]
                        {
                            new Container
                            {
                                Anchor = Anchor.BottomRight,
                                Origin = Anchor.BottomRight,
                                RelativeSizeAxes = Axes.X,
                                Height = 50,
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = Color4.Beige
                                    },
                                    addElementButton = new AutoSizeButton
                                    {
                                        Action = () => project.AddElement(new TElement()),
                                        Text = buttonText
                                    }
                                }
                            }
                        }
                    }
                },
            };
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

        private void checkAdded(IEnumerable<ProjectElement> added)
        {
            var resultingAdded = new List<TElement>();
            foreach (var item in added)
            {
                if (item is TElement itemT && Filter(itemT))
                {
                    resultingAdded.Add(itemT);
                    allElements.Add(new TButton() { Element = itemT });
                }
            }

            ItemsAdded?.Invoke(resultingAdded);
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

            ItemsRemoved?.Invoke(resultingRemoved);
        }

        private class ObjectManagerScrollContainer : BasicScrollContainer
        {
            public ObjectManagerScrollContainer()
            {
                Scrollbar.Blending = BlendingParameters.Additive;
                Scrollbar.Child.Colour = new Color4(50, 50, 50, 255);
            }
        }
    }
}
