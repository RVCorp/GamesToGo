using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Overlays
{
    public class SplashInfoOverlay : OverlayContainer
    {
        private SpriteText spriteText;
        private Box backgroundBox;

        public SplashInfoOverlay()
        {
            RelativeSizeAxes = Axes.X;
            Height = 80;
            Anchor = Anchor.BottomCentre;
            Origin = Anchor.TopCentre;
            Child = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    backgroundBox = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                    spriteText = new SpriteText
                    {
                        RelativePositionAxes = Axes.Y,
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Margin = new MarginPadding(12),
                        Font = new FontUsage(size: 30),
                        Truncate = true,
                    }
                }
            };
        }

        public void Show(string text, Color4 color)
        {
            spriteText.Text = text;
            if (LatestTransformEndTime > Clock.CurrentTime)
            {
                backgroundBox.FadeColour(color, 300, Easing.OutCubic);
                spriteText.MoveToY(1)
                    .Then()
                    .MoveToY(0, 200, Easing.OutCubic);
            }
            else
            {
                backgroundBox.Colour = color;
            }

            if (Y == -80)
                ClearTransforms();

            this.MoveToY(-80, 400, Easing.OutCubic)
            .Delay(4000)
            .MoveToY(0, 400, Easing.OutCubic)
            .OnComplete(_ => Hide());

            Show();
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            Hide();
            ClearTransforms();
            this.MoveToY(0, 400, Easing.OutCubic)
            .OnComplete(_ => Hide());
            return true;
        }

        protected override void PopIn()
        {

        }

        protected override void PopOut()
        {

        }
    }
}
