
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace GamesToGo.Desktop.Graphics
{
    public class ProjectObjectFillFlowContainer  : FillFlowContainer
    {
        public ProjectObjectFillFlowContainer()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Direction = FillDirection.Full;
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Padding = new MarginPadding { Vertical = 10 };
            Spacing = new Vector2(20, 5);
        }
    }
}
