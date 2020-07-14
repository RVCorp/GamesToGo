using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using GamesToGo.Desktop.Overlays;
using osuTK.Graphics;
using System;

namespace GamesToGo.Desktop.Graphics
{
    public class OptionButton : BasicButton
    {
        public OptionButton(OptionItem option, Action action)
        {
            HoverFadeDuration = 150;
            Action = action;
            Text = option.Text;
            switch (option.Type)
            {
                case OptionType.Destructive:
                    HoverColour = new Color4(200, 40, 40, 255);
                    BackgroundColour = Color4.IndianRed;
                    break;
                case OptionType.Neutral:
                    HoverColour = new Color4(50, 50, 50, 255);
                    BackgroundColour = new Color4(115, 115, 115, 255);
                    break;
                case OptionType.Additive:
                    HoverColour = Color4.LightGreen;
                    BackgroundColour = Color4.DarkGreen;
                    break;
            }
            RelativeSizeAxes = Axes.X;
            Height = 80;
            Padding = new MarginPadding { Horizontal = 10 };

            SpriteText.Font = new FontUsage(size: 40);
            SpriteText.Colour = Color4.White;
        }
    }
}
