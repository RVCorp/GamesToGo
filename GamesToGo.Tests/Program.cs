using osu.Framework;
using osu.Framework.Platform;

namespace GamesToGo.Tests
{
    public static class Program
    {
        public static void Main()
        {
            using GameHost host = Host.GetSuitableHost("GamesToGo", useOsuTK: true);
            using var game = new GamesToGoTestBrowser();

            host.Run(game);
        }
    }
}
