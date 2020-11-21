using System;
using GamesToGo.App;
using osu.Framework;
using osu.Framework.Platform;

namespace Test
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            //Estas tres lineas crean la ventana base del proyecto (en el lenguaje del framework, un "juego"), y crean una ventana para el
            using DesktopGameHost host = Host.GetSuitableHost(@"GamesToGo");
            using Game program = new GamesToGoGame();
            host.Run(program);
        }
    }
}
