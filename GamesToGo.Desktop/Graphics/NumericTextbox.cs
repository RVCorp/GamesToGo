using osu.Framework.Graphics.UserInterface;

namespace GamesToGo.Desktop.Graphics
{
    public class NumericTextBox : BasicTextBox
    {
        private readonly int length;
        public NumericTextBox (int n)
        {
            length = n;
        }
        protected override bool CanAddCharacter(char character) => char.IsNumber(character) && (Text.Length < length || SelectedText.Length > 0);
    }
}
