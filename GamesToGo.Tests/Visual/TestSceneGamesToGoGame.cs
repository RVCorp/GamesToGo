using GamesToGo.Game;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Input;
using osu.Framework.Platform;
using osu.Framework.Testing;

namespace GamesToGo.Tests.Visual
{
    public class TestSceneGamesToGoGame : TestScene
    {
        private GamesToGoGame game;

        [Resolved]
        private GamesToGoTestBrowser browser { get; set; }

        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            game = new GamesToGoGame
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                FillAspectRatio = 1080f / 1920,
                RelativeSizeAxes = Axes.Both,
                FillMode = FillMode.Fit,
                Masking = true,
            };
            game.SetHost(host);

            AddUntilStep("Wait for load", () => game.IsLoaded);
            AddStep("Toggle Draw Visualiser", () => browser.OnPressed(FrameworkAction.ToggleDrawVisualiser));

            Add(game);
        }
    }
}
