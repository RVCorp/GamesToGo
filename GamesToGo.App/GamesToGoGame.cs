using GamesToGo.App.Online;
using GamesToGo.App.Screens;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;

namespace GamesToGo.App
{
    public class GamesToGoGame : Game
    {
        private DependencyContainer dependencies;

        private ScreenStack stack;

        public GamesToGoGame()
        {
            //Titulo de la ventana
            Name = "GamesToGo";
        }

        // Para poder añadir nuevas dependencias debemos de crear un contenedor de dependencias que se base en el contenedor que nos
        // proporciona el framework por defecto
        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        private APIController api;

        private DrawSizePreservingFillContainer content;

        [BackgroundDependencyLoader]
        private void load(Storage store)
        {
            Padding = new MarginPadding() { Top = 100 };
            Resources.AddStore(new DllResourceStore(@"GamesToGo.App.dll"));
            Textures.AddStore(Host.CreateTextureLoaderStore(new OnlineStore()));
            Textures.AddStore(Host.CreateTextureLoaderStore(new StorageBackedResourceStore(store)));
            Textures.AddExtension("");
            dependencies.CacheAs(this);
            base.Content.Add(content = new DrawSizePreservingFillContainer { TargetDrawSize = new Vector2(1080, 1920) , Strategy =  DrawSizePreservationStrategy.Minimum});
            content.Add(stack = new ScreenStack { RelativeSizeAxes = Axes.Both, Depth = 0 });
            content.Add(api = new APIController());
            dependencies.Cache(api);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            //Cargamos asincronamente la pantalla de inicio de sesión y la agregamos al inicio de nuestra pila.
            LoadComponentAsync(new SessionStartScreen(), stack.Push);
        }
    }
}
