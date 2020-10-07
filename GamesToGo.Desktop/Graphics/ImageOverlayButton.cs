using GamesToGo.Desktop.Overlays;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;

namespace GamesToGo.Desktop.Graphics
{
    public class ImageOverlayButton : Button
    {
        private Box hoverBox;
        [BackgroundDependencyLoader]
        private void load()
        {
            Width = ImageFinderOverlay.ENTRY_WIDTH;
            Masking = true;
            CornerRadius = 4;
            Children = new[]
            {
                new Box
                {
                    Colour = new Colour4(145, 151, 243, 255),
                    RelativeSizeAxes = Axes.Both,
                },
                hoverBox = new Box
                {
                    Colour = Colour4.White,
                    Alpha = 0,
                    RelativeSizeAxes = Axes.Both,
                },
            };
        }

        protected override bool OnClick(ClickEvent e)
        {
            hoverBox.FadeIn()
                .Then()
                .FadeOut(250);
            return base.OnClick(e);
        }

        protected override bool OnHover(HoverEvent e)
        {
            hoverBox.FadeTo(0.2f, 125);
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            hoverBox.FadeOut(125);
        }
    }
}
