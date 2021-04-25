using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace GamesToGo.Editor.Graphics
{
    public class NumberSelectionDescriptor : ArgumentSelectionDescriptor
    {
        private SpriteText text;

        [Resolved]
        private ArgumentSelectorOverlay selectorOverlay { get; set; }
        private NumberSelector dropdown;

        [BackgroundDependencyLoader]
        private void load()
        {
            SelectionContainer.Add(text = new SpriteText
            {
                Anchor = Anchor.CentreLeft,
                Origin = Anchor.CentreLeft,
                Font = new FontUsage(size: 25),
            });
            Current.BindValueChanged(v => text.Text = v.NewValue?.ToString() ?? string.Empty, true);

            Action = () =>
            {
                dropdown = (NumberSelector)Activator.CreateInstance(typeof(NumberSelector), (object)Current);
                dropdown.Position = ToSpaceOfOtherDrawable(new Vector2(Width / 2, DrawHeight), selectorOverlay);
                selectorOverlay.Show(dropdown);
            };
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            if (selectorOverlay.State.Value == Visibility.Visible && dropdown == selectorOverlay.Current)
                Schedule(() => dropdown.Position = ToSpaceOfOtherDrawable(new Vector2(Width / 2, DrawHeight), selectorOverlay));
        }

        private class NumberSelector : Container
        {
            private const int default_max_slider_value = 10;
            private const int default_min_slider_value = 1;

            private readonly Bindable<int?> target = new Bindable<int?>();
            private NumericTextBox textBox;

            private readonly BindableInt sliderValue = new BindableInt
            {
                MinValue = default_min_slider_value,
            };

            public NumberSelector(Bindable<int?> target)
            {
                this.target.BindTo(target);
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                AutoSizeAxes = Axes.Both;
                Origin = Anchor.TopCentre;

                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0.5f,
                        Colour = Colour4.Black,
                    },
                    new FillFlowContainer
                    {
                        Direction = FillDirection.Horizontal,
                        AutoSizeAxes = Axes.Both,
                        Spacing = new Vector2(4),
                        Padding = new MarginPadding { Left = 4 },
                        Children = new Drawable[]
                        {
                            new BasicSliderBar<int>
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Size = new Vector2(100, 13),
                                BackgroundColour = Colour4.DimGray,
                                SelectionColour = Colour4.White,
                                Current = sliderValue,
                                TransferValueOnCommit = true,
                            },
                            textBox = new NumericTextBox
                            {
                                Anchor = Anchor.CentreLeft,
                                Origin = Anchor.CentreLeft,
                                Size = new Vector2(65, 35),
                                LengthLimit = 5,
                            },
                        },
                    },
                };

                textBox.Current.Value = target.Value ?? default_min_slider_value;
                sliderValue.MaxValue = Math.Max(target.Value ?? default_max_slider_value, default_max_slider_value);
                sliderValue.Value = target.Value ?? default_min_slider_value;

                textBox.OnCommit += delegate { changeValue((int)textBox.Current.Value); };
                sliderValue.BindValueChanged(c => changeValue(c.NewValue));
            }

            private void changeValue(int newValue)
            {
                if (newValue < default_min_slider_value)
                    throw new ArgumentException($"A value was set to a Number Argument which is lower than 1 (value:{newValue})");

                sliderValue.MaxValue = newValue > default_max_slider_value ? newValue : default_max_slider_value;

                textBox.Current.Value = newValue;
                sliderValue.Value = newValue;
                target.Value = newValue;
            }
        }
    }
}
