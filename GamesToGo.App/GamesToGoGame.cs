using GamesToGo.App.Online;
using GamesToGo.App.Screens;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Screens;

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
    }
}
