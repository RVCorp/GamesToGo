using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Graphics.UserInterface;

namespace GamesToGo.Desktop.Graphics
{
    public class NumericTextbox : BasicTextBox
    {
        protected override bool CanAddCharacter(char character) => char.IsNumber(character) && Text.Length < 2;
    }
}
