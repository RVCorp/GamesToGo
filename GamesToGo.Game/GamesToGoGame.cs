using System.Collections.Specialized;
using System.IO;
using System.Linq;
using GamesToGo.Common.Online;
using GamesToGo.Common.Online.RequestModel;
using GamesToGo.Common.Online.Requests;
using GamesToGo.Common.Overlays;
using GamesToGo.Game.Online.Models.RequestModel;
using GamesToGo.Game.Screens;
using Ionic.Zip;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
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
    [Cached]
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

        private SplashInfoOverlay infoOverlay;

        [Cached]
        public readonly BindableList<Invitation> Invitations = new BindableList<Invitation>();

        private SessionStartScreen startScreen;

        [Resolved]
        private Storage store { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            Resources.AddStore(new DllResourceStore(@"GamesToGo.Game.dll"));
            Textures.AddStore(Host.CreateTextureLoaderStore(new OnlineStore()));
            Textures.AddStore(Host.CreateTextureLoaderStore(new StorageBackedResourceStore(store)));
            dependencies.CacheAs(this);
            base.Content.Add(content = new DrawSizePreservingFillContainer
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
            dependencies.Cache(stack);
            content.Add(infoOverlay = new SplashInfoOverlay(SplashPosition.Top, 150, 60) { Depth = -1 });
            dependencies.Cache(infoOverlay);

            Invitations.BindCollectionChanged((_, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        infoOverlay.Show(@$"{e.NewItems.Cast<Invitation>().Last().Sender.Username} te ha invitado a jugar", Colour4.LightBlue);

                        break;
                    case NotifyCollectionChangedAction.Remove:
                        break;
                }
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            //Cargamos asincronamente la pantalla de inicio de sesión y la agregamos al inicio de nuestra pila.
            LoadComponentAsync(startScreen = new SessionStartScreen(), stack.Push);
        }

        public void DownloadGame(OnlineGame game)
        {
            var getGame = new DownloadProjectRequest(game.Id, game.Hash, store);
            getGame.Success += g => importGame(game);
            getGame.Progressed += g =>
            {
                infoOverlay.Show(@"Descargando juego...", Colour4.Turquoise);
            };
            api.Queue(getGame);
        }

        private void importGame(OnlineGame onlineGame)
        {
            string filename = store.GetFullPath(Path.Combine(@"download", @$"{onlineGame.Hash}.zip"));

            using var fileStream = store.GetStream(filename, FileAccess.Read, FileMode.Open);

            using ZipFile zip = ZipFile.Read(fileStream);

            foreach (ZipEntry e in zip)
                e.Extract(store.GetFullPath("files"), ExtractExistingFileAction.DoNotOverwrite);
        }

        public void Logout()
        {
            api.Logout();
            startScreen.MakeCurrent();
        }

        protected override bool OnExiting()
        {
            Logout();
            return base.OnExiting();
        }
    }
}
