using System;
using osu.Framework.Bindables;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Graphics
{
    public class EditorTabChanger : TabControl<EditorScreenOption>
    {
        public readonly Bindable<EditorScreenOption> Options = new Bindable<EditorScreenOption>();
        public EditorTabChanger()
        {
            Height = 30;
            AutoSizeAxes = Axes.X;
            TabContainer.RelativeSizeAxes &= ~Axes.X;
            TabContainer.AutoSizeAxes = Axes.X;
            TabContainer.Spacing = new Vector2(10, 0);

            foreach (var val in (EditorScreenOption[])Enum.GetValues(typeof(EditorScreenOption)))
                AddItem(val);
        }

        protected virtual bool AddEnumEntriesAutomatically => true;

        protected override Dropdown<EditorScreenOption> CreateDropdown() => null;

        protected override TabItem<EditorScreenOption> CreateTabItem(EditorScreenOption value) => new EditorTabItem(value);

        private class EditorTabItem : TabItem<EditorScreenOption>
        {
            private readonly Box colorBox;

            public EditorTabItem(EditorScreenOption value) : base(value)
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
                        Text = value.GetDescription(),
                        Font = new FontUsage(size: 20)
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
