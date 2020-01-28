using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Configuration;
using osu.Framework.Graphics.Performance;
using osu.Framework.Screens;
using GamesToGo.Desktop.Screens;

namespace GamesToGo.Desktop
{
    public class GamesToGoEditor : Game
    {
        private DependencyContainer dependencies;
        private ScreenStack stack;

        public GamesToGoEditor()
        {
            Name = "GamesToGo";
        }

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        [BackgroundDependencyLoader]
        private void load(FrameworkConfigManager config)
        {
            config.GetBindable<WindowMode>(FrameworkSetting.WindowMode).Value = WindowMode.Borderless;

            FrameStatistics.Value = FrameStatisticsMode.None;

            dependencies.CacheAs(this);
            Add(stack = new ScreenStack() { RelativeSizeAxes = Axes.Both });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            LoadComponentAsync(new SessionStartScreen(), stack.Push);
        }
    }
}
