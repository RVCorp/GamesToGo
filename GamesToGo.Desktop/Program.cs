using System;
using System.Linq;
using GamesToGo.Common.Online;
using GamesToGo.Editor;
using osu.Framework;
using osu.Framework.Platform;

namespace GamesToGo.Desktop
{
    public static class Program
    {
        [STAThread]
        public static void Main(params string[] args)
        {
            string server;
            if ((server = args.FirstOrDefault(a => a.StartsWith(@"--server="))) != null)
                APIController.AlternativeServer = server.Substring(server.IndexOf('=') + 1);
            //Estas tres lineas crean la ventana base del proyecto (en el lenguaje del framework, un "juego"), y crean una ventana para el
            using DesktopGameHost host = Host.GetSuitableHost(@"GamesToGo", useOsuTK: true);
            using Game program = new GamesToGoEditor();
                host.Run(program);
        }
    }
}
