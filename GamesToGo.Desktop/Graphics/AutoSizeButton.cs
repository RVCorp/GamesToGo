using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Graphics
{
    public class AutoSizeButton : GamesToGoButton
    {
        private readonly Container content = new Container
        {
            RelativeSizeAxes = Axes.Y,
            AutoSizeAxes = Axes.X,
        };

        protected override Container<Drawable> Content => content;
        public AutoSizeButton()
        {
            AddInternal(content);
            Padding = new MarginPadding(7);
            RelativeSizeAxes = Axes.Y;
            Anchor = Anchor.BottomRight;
            Origin = Anchor.BottomRight;
            AutoSizeAxes = Axes.X;
            BackgroundColour = Colour4.Black;
            HoverColour = new Colour4(55, 55, 55, 255);

            content.Masking = true;
            content.CornerRadius = 5;
            content.BorderThickness = 3f;
            content.BorderColour = Colour4.Black;
        }

        protected override SpriteText CreateText()
        {
            var text = base.CreateText();

            text.Colour = Colour4.White;
            text.Font = new FontUsage(size: 25);
            text.Margin = new MarginPadding { Horizontal = 6.5f };

            return text;
        }
    }
}
