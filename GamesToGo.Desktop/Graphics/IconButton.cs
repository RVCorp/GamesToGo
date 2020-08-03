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
        private Box progressBox;
        private readonly SpriteIcon loadingIcon;

        public IconButton(bool progressAction = false)
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
                progressBox = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Color4(80, 80, 80, 255),
                    Width = 0,
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding(5),
                    Children = new[]
                    {
                        loadingIcon = new SpriteIcon
                        {
                            RelativeSizeAxes = Axes.Both,
                            Icon = FontAwesome.Solid.Spinner,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Alpha = 0,
                        },
                        icon = new SpriteIcon
                        {
                            RelativeSizeAxes = Axes.Both,
                        }
                    }
                }
            };

            Enabled.ValueChanged += e =>
            {
                if (IsHovered && e.NewValue)
                    fadeToColour();
                else
                    fadeToWait();
            };

            Action += () =>
            {
                if (progressAction)
                {
                    Enabled.Value = false;
                    loadingIcon.FadeIn(100);
                    icon.FadeOut(100);
                    fadeToWait();
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            loadingIcon.RotateTo(0).Then().RotateTo(360, 1500).Loop();
        }

        public IconUsage Icon { set => icon.Icon = value; }

        public Color4 ButtonColour { set => hoverBox.Colour = value; }

        public Color4 BackgroundColour { set => colourBox.Colour = value; }

        public Color4 ProgressColour { set => progressBox.Colour = value; }

        public float Progress { set => progressBox.Width = value; }

        private void fadeToColour()
        {
            hoverBox.FadeIn(100);
        }

        private void fadeToWait()
        {
            hoverBox.FadeOut(100);
        }

        protected override bool OnHover(HoverEvent e)
        {
            if (Enabled.Value)
                fadeToColour();
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            fadeToWait();
            base.OnHoverLost(e);
        }
    }
}
