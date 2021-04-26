using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Framework.Threading;

namespace GamesToGo.Common.Overlays
{
    public class SplashInfoOverlay : OverlayContainer
    {
        private TextFlowContainer textFlow;
        private Box backgroundBox;
        private readonly float minHeight;
        private readonly float fontSize;
        private readonly SplashPosition position;
        private Colour4 targetColour;
        private string latestText;
        private string targetText;
        private readonly BindableFloat targetHeight;
        private ScheduledDelegate delayedHide;

        public SplashInfoOverlay(SplashPosition position, float minHeight, float fontSize)
        {
            this.minHeight = minHeight;
            this.fontSize = fontSize;
            this.position = position;
            targetHeight = new BindableFloat(minHeight);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.X;
            Masking = true;

            switch (position)
            {
                case SplashPosition.Top:
                    Anchor = Anchor.TopCentre;
                    Origin = Anchor.TopCentre;
                    break;
                case SplashPosition.Bottom:
                    Anchor = Anchor.BottomCentre;
                    Origin = Anchor.BottomCentre;
                    break;
            }

            Child = new Container
            {
                RelativePositionAxes = Axes.Both,
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    backgroundBox = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                    textFlow = new TextFlowContainer(font => font.Font = new FontUsage(size: fontSize))
                    {
                        AutoSizeAxes = Axes.Y,
                        RelativeSizeAxes = Axes.X,
                        RelativePositionAxes = Axes.Y,
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Padding = new MarginPadding(12),
                    },
                },
            };

            targetHeight.BindValueChanged(e => this.ResizeHeightTo(e.NewValue, 200, Easing.OutCubic), true);
        }

        public void Show(string text, Colour4 colour)
        {
            Hide();
            delayedHide?.Cancel();
            targetText = text;
            targetColour = colour;
            Show();
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            targetHeight.Value = Math.Max(minHeight, textFlow.Height);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            Hide();

            return true;
        }

        protected override void PopIn()
        {
            if (delayedHide != null && !delayedHide.Completed)
            {
                backgroundBox.FadeColour(targetColour, 300, Easing.OutCubic);

                if (targetText != latestText)
                    textFlow.MoveToY(position == SplashPosition.Bottom ? 1 : -1)
                        .Then()
                        .MoveToY(0, 200, Easing.OutCubic);
            }
            else
            {
                backgroundBox.Colour = targetColour;
            }

            textFlow.Text = latestText = targetText;

            Child.MoveToY(0, 400, Easing.OutCubic);

            delayedHide = Scheduler.AddDelayed(Hide, 4400);
        }

        protected override void PopOut()
        {
            Child.MoveToY(position == SplashPosition.Bottom ? 1 : -1, 400, Easing.OutCubic);

            delayedHide?.Cancel();
        }
    }

    public enum SplashPosition
    {
        Top,
        Bottom,
    }
}
