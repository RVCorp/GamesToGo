using System;
using GamesToGo.Editor.Project;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace GamesToGo.Editor.Graphics
{
    public abstract class ObjectManagerContainer<TElement, TButton> : ObjectListingContainer<TElement, TButton>
        where TElement : ProjectElement, new()
        where TButton : ElementEditButton, new()
    {
        private readonly string buttonText;
        private AutoSizeButton addElementButton;

        private readonly Container content = new Container
        {
            RelativeSizeAxes = Axes.Both,
        };

        public Action ButtonAction { set => addElementButton.Action = value; }

        protected override Container<Drawable> Content => content;

        protected ObjectManagerContainer(string buttonText = @"Añadir nuevo")
        {
            this.buttonText = buttonText;
        }

        [BackgroundDependencyLoader]
        private void load(WorkingProject project)
        {
            AddInternal(new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new[]
                {
                    new Dimension(),
                    new Dimension(GridSizeMode.Absolute, 50),
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        content,
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
                                    Colour = Colour4.Beige,
                                },
                                addElementButton = new AutoSizeButton
                                {
                                    Text = buttonText ?? @"Añadir nuevo",
                                },
                            },
                        },
                    },
                },
            });

            addElementButton.Action = () => project.AddElement(new TElement());
        }
    }
}
