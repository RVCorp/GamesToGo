using GamesToGo.Common.Overlays;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Testing;

namespace GamesToGo.Tests.Visual.Common
{
    public class TestSceneSplashInfoOverlay : TestScene
    {
        private SplashInfoOverlay bottomOverlay;
        private SplashInfoOverlay topOverlay;
        private Box bottomBox;

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(bottomBox = new Box
            {
                Colour = Colour4.Black,
                RelativeSizeAxes = Axes.Both,
            });
            Add(bottomOverlay = new SplashInfoOverlay(SplashPosition.Bottom, 80, 30));
            Add(topOverlay = new SplashInfoOverlay(SplashPosition.Top, 150, 60));

            AddStep("Show short", shortText);
            AddUntilStep("Wait for hide and no transforms", allHiddenAndNoTransforms);
            AddStep("Show long", longText);
        }

        private void shortText()
        {
            bottomOverlay.Show("Short test", Colour4.Brown);
            topOverlay.Show("Short test", Colour4.Brown);
        }

        private void longText()
        {
            bottomOverlay.Show(
                "Really long text which should take more than one line to finish, probably taking even more depending on font size and overlay size, and should resize the overlay to make space for the text since we are masking to not even attempt to show text out of screen and our own dedicated space (bottom or top of screen)",
                Colour4.DarkRed);

            topOverlay.Show(
                "Really long text which should take more than one line to finish, probably taking even more depending on font size and overlay size, and should resize the overlay to make space for the text since we are masking to not even attempt to show text out of screen and our own dedicated space (bottom or top of screen)",
                Colour4.DarkRed);
        }

        private bool allHiddenAndNoTransforms()
        {
            return bottomOverlay.State.Value == Visibility.Hidden && bottomOverlay.Child.LatestTransformEndTime <= Clock.CurrentTime
                && topOverlay.State.Value == Visibility.Hidden && bottomOverlay.Child.LatestTransformEndTime <= Clock.CurrentTime;
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            bottomBox.FadeColour(new Colour4(50, 50, 50, 255))
                .FadeColour(Colour4.Black, 300, Easing.OutCubic);

            return base.OnMouseDown(e);
        }
    }
}
