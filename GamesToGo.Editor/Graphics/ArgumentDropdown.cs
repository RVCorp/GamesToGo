using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;

namespace GamesToGo.Editor.Graphics
{
    public abstract class ArgumentDropdown : Container
    {
        private FillFlowContainer<ArgumentItem> elements;
        private readonly Bindable<int?> target = new Bindable<int?>();

        protected ArgumentDropdown(Bindable<int?> target)
        {
            this.target.BindTo(target);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AutoSizeAxes = Axes.Both;
            Origin = Anchor.TopCentre;

            Child = elements = new FillFlowContainer<ArgumentItem>
            {
                Direction = FillDirection.Vertical,
                AutoSizeAxes = Axes.Both,
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            foreach (var item in CreateItems())
            {
                item.Action += () => target.Value = (int?)item.Value;
                elements.Add(item);
            }
        }

        protected abstract IEnumerable<ArgumentItem> CreateItems();

        protected class ArgumentItem : Button
        {
            protected const int ARGUMENT_HEIGHT = 25;
            private const int argument_padding = 4;
            private static readonly Colour4 hover_black = new Colour4(0.3f, 0.3f, 0.3f, 1f);

            [Resolved]
            private ArgumentSelectorOverlay selectorOverlay { get; set; }

            private readonly Container content = new Container
            {
                AutoSizeAxes = Axes.X,
                Height = ARGUMENT_HEIGHT + argument_padding,
                Padding = new MarginPadding { Vertical = argument_padding / 2f, Horizontal = argument_padding },
            };

            private Box backgroundBox;
            public readonly int Value;

            public ArgumentItem(int value)
            {
                Value = value;
            }

            protected override Container<Drawable> Content => content;

            [BackgroundDependencyLoader]
            private void load()
            {
                AutoSizeAxes = Axes.Both;
                Action += () => selectorOverlay.Hide();
                InternalChildren = new Drawable[]
                {
                    backgroundBox = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0.5f,
                        Colour = Colour4.Black,
                    },
                    content,
                };
            }

            protected override bool OnHover(HoverEvent e)
            {
                base.OnHover(e);

                backgroundBox.FadeColour(hover_black, 150);

                return true;
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                base.OnHoverLost(e);

                backgroundBox.FadeColour(Colour4.Black, 150);
            }
        }
    }
}
