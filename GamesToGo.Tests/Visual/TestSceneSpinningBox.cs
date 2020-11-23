using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Testing;
using osuTK;

namespace GamesToGo.Tests.Visual
{
    public class TestSceneSpinningBox : TestScene
    {
        private Box box;

        [BackgroundDependencyLoader]
        private void load()
        {
            Add(box = new Box
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Size = new Vector2(300),
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            box.Loop(b => b.RotateTo(0).RotateTo(360, 2500));
        }
    }
}
