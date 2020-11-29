using GamesToGo.Game.Online;
using GamesToGo.Game.Screens;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osu.Framework.Testing;
using osuTK;

namespace GamesToGo.Game
{
    [ExcludeFromDynamicCompile]
    public class GamesToGoGame : osu.Framework.Game
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
            Resources.AddStore(new DllResourceStore(@"GamesToGo.Game.dll"));
            Textures.AddStore(Host.CreateTextureLoaderStore(new OnlineStore()));
            Textures.AddStore(Host.CreateTextureLoaderStore(new StorageBackedResourceStore(store)));
            Textures.AddExtension("");
            dependencies.CacheAs(this);
            base.Content.Add(content = new PaddedDrawSizePreservingFillContainer
            {
                TargetDrawSize = new Vector2(1080, 1920),
                Strategy =  DrawSizePreservationStrategy.Minimum,
            });
            content.Add(stack = new ScreenStack
            {
                RelativeSizeAxes = Axes.Both,
                Depth = 0,
            });
            content.Add(api = new APIController());
            dependencies.Cache(api);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            //Cargamos asincronamente la pantalla de inicio de sesión y la agregamos al inicio de nuestra pila.
            LoadComponentAsync(new SessionStartScreen(), stack.Push);
        }

        private class PaddedDrawSizePreservingFillContainer : DrawSizePreservingFillContainer
        {
            protected override void LoadComplete()
            {
                base.LoadComplete();

                Content.Padding = new MarginPadding { Top = 100 };
            }
        }
    }
}
