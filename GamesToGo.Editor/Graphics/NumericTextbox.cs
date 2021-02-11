using System.Globalization;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.UserInterface;

namespace GamesToGo.Editor.Graphics
{
    public class NumericTextBox : BasicTextBox, IHasCurrentValue<float>
    {
        public override string Text
        {
            get => base.Text;
            set
            {
                if (!float.TryParse(value, out var number))
                {
                    NotifyInputError();

                    return;
                }

                current.Value = number;
                base.Text = value;
            }
        }

        private readonly bool zero;

        private readonly Bindable<float> current = new BindableFloat();

        public new Bindable<float> Current { get; set; } = new BindableFloat(1);

        public NumericTextBox(bool allowZero = true)
        {
            zero = allowZero;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            current.BindValueChanged(numChanged =>
            {
                if (numChanged.NewValue == 0 && !zero)
                {
                    Current.Value = numChanged.OldValue;

                    if (IsLoaded)
                        KillFocus();
                }
                else
                    base.Current.Value = numChanged.NewValue.ToString(CultureInfo.InvariantCulture);
            }, true);

            Current.BindTo(current);
        }

        protected override void OnTextCommitted(bool textChanged)
        {
            if (string.IsNullOrEmpty(Text))
                Text = Current.Value.ToString(CultureInfo.InvariantCulture);

            float value = float.Parse(Text);

            if (value == 0 && !zero)
            {
                Text = Current.Value.ToString(CultureInfo.InvariantCulture);
                NotifyInputError();
            }

            current.Value = value;
        }

        protected override bool CanAddCharacter(char character) => char.IsNumber(character);
    }
}
