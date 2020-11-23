using GamesToGo.Editor;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Framework.Testing;

namespace GamesToGo.Tests.Visual
{
    public class TestSceneGamesToGoDesktop : TestScene
    {
        private GamesToGoEditor game;

        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            game = new GamesToGoEditor
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                FillAspectRatio = 1920f / 1080,
                RelativeSizeAxes = Axes.Both,
                FillMode = FillMode.Fit,
                Masking = true,
            };
            game.SetHost(host);

            Add(game);
        }
    }
}
