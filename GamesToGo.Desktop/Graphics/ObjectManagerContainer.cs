using System;
using GamesToGo.Desktop.Project;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace GamesToGo.Desktop.Graphics
{
    public abstract class ObjectManagerContainer<TElement, TButton> : ObjectListingContainer<TElement, TButton>
        where TElement : ProjectElement, new()
        where TButton : ElementEditButton, new()
    {
        private AutoSizeButton addElementButton;

        private Container content;

        public Action ButtonAction { set => addElementButton.Action = value; }

        protected override Container<Drawable> Content => content;

        public ObjectManagerContainer(string buttonText = "Añadir nuevo")
        {
            AddInternal(new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new Dimension[]
                {
                    new Dimension(),
                    new Dimension(GridSizeMode.Absolute, 50), 
                },
                Content = new Drawable[][]
                {
                    new Drawable[]
                    {
                        content = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                        },
                    },
                    new Drawable[]
                    {
                        new Container
                        {
                            Anchor = Anchor.BottomRight,
                            Origin = Anchor.BottomRight,
                            RelativeSizeAxes = Axes.Both,
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = Colour4.Beige
                                },
                                addElementButton = new AutoSizeButton
                                {
                                    Text = buttonText ?? "Añadir nuevo"
                                }
                            }
                        }
                    },
                }
            });

        }

        [BackgroundDependencyLoader]
        private void load(WorkingProject project)
        {
            addElementButton.Action = () => project.AddElement(new TElement());
        }
    }
}
