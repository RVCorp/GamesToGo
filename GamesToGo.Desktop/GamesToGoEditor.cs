using System;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Configuration;
using osu.Framework.Screens;
using GamesToGo.Desktop.Screens;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Linq;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using GamesToGo.Desktop.Overlays;
using GamesToGo.Desktop.Project;
using System.Reflection;
using GamesToGo.Desktop.Database;
using GamesToGo.Desktop.Online;
using GamesToGo.Desktop.Project.Actions;
using GamesToGo.Desktop.Project.Arguments;
using GamesToGo.Desktop.Project.Events;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace GamesToGo.Desktop
{
    /// <summary>
    /// Contenedor base de la aplicación, dede de cargar e incluir lo necesario para que el programa pueda cargar
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
        private APIController api;
        private ImageFinderOverlay imageFinder;
        private DrawSizePreservingFillContainer content;

        //Cargar dependencias, configuración, etc., necesarias para el proyecto.
        [BackgroundDependencyLoader]
        private void load(FrameworkConfigManager config, Storage store) //Esta es la manera en la que se acceden a elementos de las dependencias, su tipo y un nombre local.
        {
            Host.Window.Title = Name;
            Resources.AddStore(new DllResourceStore(@"GamesToGo.Desktop.dll"));
            Textures.AddStore(Host.CreateTextureLoaderStore(new OnlineStore()));
            Textures.AddStore(Host.CreateTextureLoaderStore(new StorageBackedResourceStore(store)));
            Textures.AddExtension("");
            dependencies.CacheAs(dbContext = new Context(Host.Storage.GetDatabaseConnectionString(Name)));

            previewEvents();
            previewActions();
            previewArguments();

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
                foreach (var _ in dbContext.Projects)
                {
                }
                foreach (var _ in dbContext.Files)
                {
                }
                foreach (var _ in dbContext.Relations)
                {
                }
            }

            //Ventana sin bordes, sin requerir modo exclusivo.
            config.GetBindable<WindowMode>(FrameworkSetting.WindowMode).Value = WindowMode.Borderless;
            config.GetBindable<FrameSync>(FrameworkSetting.FrameSync).Value = FrameSync.VSync;

            //Para agregar un elemento a las dependencias se agrega a su caché. En este caso se agrega el "juego" como un GamesToGoEditor
            dependencies.CacheAs(this);

            base.Content.Add(content = new DrawSizePreservingFillContainer { TargetDrawSize = new Vector2(1920, 1080) });

            //Cargamos y agregamos nuestra pila de pantallas a la ventana.
            content.Add(stack = new ScreenStack { RelativeSizeAxes = Axes.Both, Depth = 0 });

            content.Add(api = new APIController());

            dependencies.Cache(api);

            content.Add(splashOverlay = new SplashInfoOverlay { Depth = -2 });

            dependencies.Cache(splashOverlay);

            content.Add(imageFinder = new ImageFinderOverlay { Depth = -1 });

            dependencies.Cache(imageFinder);

            content.Add(optionsOverlay = new MultipleOptionOverlay { Depth = -3 });

            dependencies.Cache(optionsOverlay);
        }

        private static void previewEvents()
        {
            foreach (Type type in Assembly.GetAssembly(typeof(ProjectEvent))?.GetTypes()?.Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(ProjectEvent))))
            {
                if (!(Activator.CreateInstance(type) is ProjectEvent thing))
                    continue;

                try
                {
                    WorkingProject.AvailableEvents.Add(thing.TypeID, type);
                }
                catch (ArgumentException e)
                {
                    throw new ArgumentException($"{nameof(thing)} has the same id as {WorkingProject.AvailableEvents[thing.TypeID].Name}, can't resolve IDs", e);
                }
            }
        }

        private static void previewActions()
        {
            foreach (Type type in Assembly.GetAssembly(typeof(EventAction))?.GetTypes()?.Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(EventAction))))
            {
                if (!(Activator.CreateInstance(type) is EventAction thing))
                    continue;

                try
                {
                    WorkingProject.AvailableActions.Add(thing.TypeID, type);
                }
                catch (ArgumentException e)
                {
                    throw new ArgumentException($"{nameof(thing)} has the same id as {WorkingProject.AvailableActions[thing.TypeID].Name}, can't resolve IDs", e);
                }
            }
        }

        private static void previewArguments()
        {
            foreach (Type type in Assembly.GetAssembly(typeof(Argument))?.GetTypes()?.Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(Argument))))
            {
                if (!(Activator.CreateInstance(type) is Argument thing))
                    continue;

                try
                {
                    WorkingProject.AvailableArguments.Add(thing.ArgumentTypeID, type);
                }
                catch (ArgumentException e)
                {
                    throw new ArgumentException($"{nameof(thing)} has the same id as {WorkingProject.AvailableArguments[thing.ArgumentTypeID].Name}, can't resolve IDs", e);
                }

            }
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
