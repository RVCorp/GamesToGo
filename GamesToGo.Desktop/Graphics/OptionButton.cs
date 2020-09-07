using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using GamesToGo.Desktop.Overlays;
using System;

namespace GamesToGo.Desktop.Graphics
{
    public class OptionButton : GamesToGoButton
    {
        public OptionButton(OptionItem option, Action action)
        {
            HoverFadeDuration = 150;
            Action = action;
            Text = option.Text;
            switch (option.Type)
            {
                case OptionType.Destructive:
                    HoverColour = new Colour4(200, 40, 40, 255);
                    BackgroundColour = Colour4.IndianRed;
                    break;
                case OptionType.Neutral:
                    HoverColour = new Colour4(50, 50, 50, 255);
                    BackgroundColour = new Colour4(115, 115, 115, 255);
                    break;
                case OptionType.Additive:
                    HoverColour = Colour4.Green.Opacity(150);
                    BackgroundColour = Colour4.DarkGreen;
                    break;
            }
            RelativeSizeAxes = Axes.X;
            Height = 80;
            Padding = new MarginPadding { Horizontal = 10 };

            SpriteText.Font = new FontUsage(size: 40);
            SpriteText.Colour = Colour4.White;
        }
    }
}
