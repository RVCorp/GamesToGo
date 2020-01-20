using System;
using osu.Framework;
using osu.Framework.Platform;

namespace GamesToGo.Desktop
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            using (DesktopGameHost host = Host.GetSuitableHost(@"GamesToGo"))
            using (Game game = new GamesToGoEditor())
                host.Run(game);
        }
    }
}
