using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;

namespace GamesToGo.Editor.Graphics
{
    public class IconButton : Button
    {
        private readonly IconUsage buttonIcon;
        private readonly Colour4 backgroundColour;
        private readonly Colour4 buttonColour;
        private readonly Colour4 progressColour;
        private SpriteIcon icon;
        private Box hoverBox;
        private Box progressBox;
        private SpriteIcon loadingIcon;


        public IconButton(IconUsage buttonIcon, Colour4 buttonColour = default, bool progressAction = false, Colour4 progressColour = default, Colour4 backgroundColour = default)
        {
            this.buttonIcon = buttonIcon;
            this.backgroundColour = backgroundColour == default ? new Colour4(100, 100, 100, 255) : backgroundColour;
            this.buttonColour = buttonColour == default ? new Colour4(100, 100, 100, 255) : buttonColour;
            this.progressColour = progressColour == default ? new Colour4(80, 80, 80, 255) : progressColour;

            if (!progressAction) return;

            Action += () =>
            {
                Enabled.Value = false;
                loadingIcon.FadeIn(100);
                icon.FadeOut(100);
                fadeToWait();
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Masking = true;
            CornerRadius = 5;
            Anchor = Anchor.CentreRight;
            Origin = Anchor.CentreRight;
            Size = new Vector2(35 * 1.5f, 35);
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = backgroundColour,
                },
                hoverBox = new Box
                {
                    Alpha = 0,
                    RelativeSizeAxes = Axes.Both,
                    Colour = buttonColour,
                },
                progressBox = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = progressColour,
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
                            Icon = buttonIcon,
                        },
                    },
                },
            };

            Enabled.ValueChanged += e =>
            {
                if (IsHovered && e.NewValue)
                    fadeToColour();
                else
                    fadeToWait();
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            loadingIcon.RotateTo(0).Then().RotateTo(360, 1500).Loop();
        }

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
