using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace GamesToGo.Desktop.Graphics
{
    public class LabeledElement<TElement, TValue> : Container, IHasCurrentValue<TValue> where TElement : Drawable, IHasCurrentValue<TValue>
    {
        public Bindable<TValue> Current
        {
            get => Element.Current;
            set => Element.Current = value;
        }

        private TElement element;

        public TElement Element
        {
            get => element;
            set
            {
                if (elementContainer != null)
                    elementContainer.Child = Element;

                element = value;
            }
        }

        private string text;

        public string Text
        {
            get => text;
            set
            {
                if (spriteText != null)
                    spriteText.Text = value;

                text = value;
            }
        }

        private SpriteText spriteText;
        private Container<TElement> elementContainer;

        [BackgroundDependencyLoader]
        private void load()
        {
            AutoSizeAxes = Axes.Both;

            Child = new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Direction = FillDirection.Horizontal,
                Spacing = new Vector2(10),
                Children = new Drawable[]
                {
                    spriteText = new SpriteText
                    {
                        Text = text,
                    },
                    elementContainer = new Container<TElement>
                    {
                        AutoSizeAxes = Axes.Both,
                        Child = Element,
                    },
                },
            };
        }
    }
}
