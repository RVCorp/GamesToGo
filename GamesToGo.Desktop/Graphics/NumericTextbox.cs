using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Graphics.UserInterface;

namespace GamesToGo.Desktop.Graphics
{
    public class NumericTextbox : BasicTextBox
    {
        private int length;
        public NumericTextbox (int n)
        {
            length = n;
        }
        protected override bool CanAddCharacter(char character) => char.IsNumber(character) && (Text.Length < length || SelectedText.Length > 0);
    }
}
