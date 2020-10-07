using System;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace GamesToGo.Desktop.Graphics
{
    public class EditorTabChanger : TabControl<EditorScreenOption>
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            Height = 30;
            AutoSizeAxes = Axes.X;
            TabContainer.RelativeSizeAxes &= ~Axes.X;
            TabContainer.AutoSizeAxes = Axes.X;
            TabContainer.Spacing = new Vector2(10, 0);

            foreach (var val in (EditorScreenOption[])Enum.GetValues(typeof(EditorScreenOption)))
                AddItem(val);
        }

        protected override Dropdown<EditorScreenOption> CreateDropdown() => null;

        protected override TabItem<EditorScreenOption> CreateTabItem(EditorScreenOption value) => new EditorTabItem(value);

        private class EditorTabItem : TabItem<EditorScreenOption>
        {
            private Box colorBox;

            public EditorTabItem(EditorScreenOption value) : base(value) { }

            [BackgroundDependencyLoader]
            private void load()
            {
                AutoSizeAxes = Axes.X;
                RelativeSizeAxes = Axes.Y;

                Children = new Drawable[]
                {
                    colorBox = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.Red,
                        Alpha = 0,
                    },
                    new SpriteText
                    {
                        Margin = new MarginPadding { Horizontal = 5 },
                        Origin = Anchor.CentreLeft,
                        Anchor = Anchor.CentreLeft,
                        Text = Value.GetDescription(),
                        Font = new FontUsage(size: 20),
                    },
                };
            }
            protected override void OnActivated()
            {
                colorBox.FadeIn(125);
            }

            protected override void OnDeactivated()
            {
                colorBox.FadeOut(125);
            }
        }
    }
}
