using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Graphics
{
    public class IconButton : Button
    {
        private readonly SpriteIcon icon;
        private readonly Box colourBox;
        private Box hoverBox;

        public IconButton()
        {
            Masking = true;
            CornerRadius = 5;
            Anchor = Anchor.CentreRight;
            Origin = Anchor.CentreRight;
            Size = new Vector2(35 * 1.5f, 35);
            Children = new Drawable[]
            {
                    colourBox = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = new Color4(100, 100, 100, 255),
                    },
                    hoverBox = new Box
                    {
                        Alpha = 0,
                        RelativeSizeAxes = Axes.Both,
                        Colour = new Color4(100, 100, 100, 255),
                    },
                    new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Padding = new MarginPadding(5),
                        Child = icon = new SpriteIcon
                        {
                            RelativeSizeAxes = Axes.Both,
                        }
                    }
            };

            Enabled.ValueChanged += e =>
            {
                if (IsHovered && e.NewValue)
                    fadeToColour();
            };
        }

        public IconUsage Icon { set => icon.Icon = value; }

        public Color4 ButtonColour { set => hoverBox.Colour = value; }

        public Color4 BackgroundColour { set => colourBox.Colour = value; }

        private void fadeToColour()
        {
            hoverBox.FadeIn(100);
        }

        protected override bool OnHover(HoverEvent e)
        {
            if (Enabled.Value)
                fadeToColour();
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            hoverBox.FadeOut(100);
            base.OnHoverLost(e);
        }
    }
}
