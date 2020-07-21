using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Configuration;
using osu.Framework.Screens;
using GamesToGo.Desktop.Screens;
using GamesToGo.Desktop.Database.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Linq;
using osu.Framework.IO.Stores;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using GamesToGo.Desktop.Overlays;
using GamesToGo.Desktop.Project;
using System.Reflection;

namespace GamesToGo.Desktop
{
    /// <summary>
    /// Contenedor base de la aplicación, dede de cargar e incluir todo lo necesario para que el programa pueda cargar
    /// </summary>
    public class GamesToGoEditor : Game
    {
        // El contenedor de dependencias es, en lenguaje del framework, el contenedor de objetos que pueden ser obtenidos por otros
        // contenedores sin tener una referencia directa.
        private DependencyContainer dependencies;

        // La pila de pantallas permite acciones en las pantallas del proyecto (Cambio y flujo de pantallas, comprobar orden, etc.)
        private ScreenStack stack;

        public GamesToGoEditor()
        {
            //Titulo de la ventana
            Name = "GamesToGo";
        }

        // Para poder añadir nuevas dependencias debemos de crear un contenedor de dependencias que se base en el contenedor que nos
        // proporciona el framework por defecto
        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        private Context dbContext;
        private MultipleOptionOverlay optionsOverlay;
        private SplashInfoOverlay splashOverlay;

        //Cargar dependencias, configuración, etc., necesarias para el proyecto.
        [BackgroundDependencyLoader]
        private void load(FrameworkConfigManager config, Storage store) //Esta es la manera en la que se acceden a elementos de las dependencias, su tipo y un nombre local.
        {
            Resources.AddStore(new DllResourceStore(@"GamesToGo.Desktop.dll"));
            Textures.AddStore(Host.CreateTextureLoaderStore(new NamespacedResourceStore<byte[]>(Resources, @"files")));
            Textures.AddExtension("");
            dependencies.CacheAs(dbContext = new Context(Host.Storage.GetDatabaseConnectionString(Name)));

            var largeStore = new LargeTextureStore(Host.CreateTextureLoaderStore(new NamespacedResourceStore<byte[]>(Resources, @"Textures")));
            dependencies.Cache(largeStore);

            try
            {
                dbContext.Database.Migrate();
            }
            catch
            {
                Host.Storage.DeleteDatabase(Name);
                dbContext.Database.Migrate();
                store.DeleteDirectory("files");
            }
            finally
            {
                foreach (var _ in dbContext.Projects) ;
                foreach (var _ in dbContext.Files) ;
                foreach (var _ in dbContext.Relations) ;
            }

            //Ventana sin bordes, sin requerir modo exclusivo.
            config.GetBindable<WindowMode>(FrameworkSetting.WindowMode).Value = WindowMode.Borderless;
            config.GetBindable<FrameSync>(FrameworkSetting.FrameSync).Value = FrameSync.VSync;

            //Para agregar un elemento a las dependencias se agrega a su caché. En este caso se agrega el "juego" como un GamesToGoEditor
            dependencies.CacheAs(this);

            //Cargamos y agregamos nuestra pila de pantallas a la ventana.
            Add(stack = new ScreenStack() { RelativeSizeAxes = Axes.Both });

            Add(splashOverlay = new SplashInfoOverlay());

            dependencies.Cache(splashOverlay);

            Add(optionsOverlay = new MultipleOptionOverlay());

            dependencies.Cache(optionsOverlay);
        }

        public override void SetHost(GameHost host)
        {
            base.SetHost(host);

            switch (host.Window)
            {
                case DesktopGameWindow gameWindow:
                    gameWindow.SetIconFromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream(GetType(), "gtg.ico"));
                    break;
            }
        }

        //Corrido cuando se termine de cargar el juego, justo antes de ser renderizado.
        protected override void LoadComplete()
        {
            base.LoadComplete();

            ProjectElement.Textures = Textures;

            //Cargamos asincronamente la pantalla de inicio de sesión y la agregamos al inicio de nuestra pila.
            LoadComponentAsync(new SessionStartScreen(), stack.Push);
        }

        public static string HashBytes(byte[] bytes)
        {
            using SHA1Managed hasher = new SHA1Managed();
            return string.Concat(hasher.ComputeHash(bytes).Select(by => by.ToString("X2")));
        }
    }
}
