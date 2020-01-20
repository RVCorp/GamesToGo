using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics;
using osu.Framework.Configuration;
using osu.Framework.Graphics.Performance;

namespace GamesToGo.Desktop
{
    public class GamesToGoEditor : Game
    {
        private SpriteText text;

        public GamesToGoEditor()
        {
            Name = "GamesToGo";
        }

        [BackgroundDependencyLoader]
        private void load(FrameworkConfigManager config)
        {
            config.GetBindable<WindowMode>(FrameworkSetting.WindowMode).Value = WindowMode.Borderless;

            FrameStatistics.Value = FrameStatisticsMode.None;

            Add(text = new SpriteText
            {
                Text = "Start",
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre
            });
        }


    }
}
