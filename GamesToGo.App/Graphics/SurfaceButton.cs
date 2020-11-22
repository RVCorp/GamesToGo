using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;

namespace GamesToGo.App.Graphics
{
    public class SurfaceButton : Button
    {
        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
        }
    }
}
