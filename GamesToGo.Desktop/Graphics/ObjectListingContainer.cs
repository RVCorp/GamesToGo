using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
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
    [SuppressMessage("ReSharper", "ConvertIfStatementToReturnStatement")]
    public class ObjectListingContainer<TElement, TButton> : Container
        where TElement : ProjectElement
        where TButton : ElementEditButton, new()
    {
        private readonly IBindableList<ProjectElement> localElements = new BindableList<ProjectElement>();

        private FillFlowContainer<TButton> allElements;

        private Func<TElement, bool> filter = _ => true;
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

        private static Colour4 BackgroundColour
        {
            get
            {
                if (typeof(TElement) == typeof(Card))
                    return Colour4.Maroon;
                if (typeof(TElement) == typeof(Token))
                    return Colour4.Crimson;
                if (typeof(TElement) == typeof(Board))
                    return Colour4.DarkSeaGreen;
                if (typeof(TElement) == typeof(Tile))
                    return Colour4.BlueViolet;
                return Colour4.Black;
            }
        }

        private static string Title
        {
            get
            {
                if (typeof(TElement) == typeof(Card))
                    return @"Cartas";
                if (typeof(TElement) == typeof(Token))
                    return @"Fichas";
                if (typeof(TElement) == typeof(Board))
                    return @"Tableros";
                if (typeof(TElement) == typeof(Tile))
                    return @"Casillas";
                return @"Elementos raros";
            }
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Add(new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new[]
                {
                    new Dimension(GridSizeMode.AutoSize),
                    new Dimension(),
                },
                Content = new[]
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
                            },
                        },
                    },
                },
            });
        }

        protected virtual void EditTButton(TButton button)
        {

        }

        private void checkAdded(IEnumerable<ProjectElement> added)
        {
            foreach (var item in added)
            {
                if (!(item is TElement itemT))
                    continue;
                if (Filter(itemT))
                {
                    TButton button = new TButton { Element = itemT };
                    EditTButton(button);
                    allElements.Add(button);
                }
            }
        }

        private void checkRemoved(IEnumerable<ProjectElement> removed)
        {
            foreach (var item in removed)
            {
                if (!(item is TElement itemT) || !Filter(itemT))
                    continue;

                var deletable = allElements.FirstOrDefault(b => b.Element.ID == item.ID);

                if (deletable != null)
                    allElements.Remove(deletable);
            }
        }

        protected void BindToList(IBindableList<ProjectElement> elements)
        {
            localElements.UnbindAll();

            allElements.Clear();
            checkAdded(elements);

            localElements.BindTo(elements);

            localElements.CollectionChanged += (_, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        checkAdded(args.NewItems.Cast<ProjectElement>());

                        break;
                    case NotifyCollectionChangedAction.Remove:
                        checkRemoved(args.OldItems.Cast<ProjectElement>());

                        break;
                }
            };
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
