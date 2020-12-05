




using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GamesToGo.Game.Online;
using GamesToGo.Game.Overlays;
using GamesToGo.Game.Screens;
using Ionic.Zip;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;

namespace GamesToGo.Game
{
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

        private SideMenuOverlay sideMenu;

        private SplashInfoOverlay infoOverlay;
        public BindableList<Invitation> Invitations = new BindableList<Invitation>();
        private SessionStartScreen startScreen;
        [Resolved]
        private Storage store { get; set; }

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
                Strategy =  DrawSizePreservationStrategy.Minimum
            });
            content.Add(stack = new ScreenStack
            {
                RelativeSizeAxes = Axes.Both,
                Depth = 0,
            });
            content.Add(api = new APIController());
            dependencies.Cache(api);
            Task.Run(() =>
            {
                while (true)
                {
                    System.Threading.Thread.Sleep(5000);
                    var invitations = new GetAllInvitationsRequest();
                    invitations.Success += u =>
                    {
                        List<Invitation> invitations1 = Invitations.ToList();
                        for (int i = 0; i < invitations1.Count; i++)
                        {
                            if (u.All(t => t.ID != invitations1[i].ID))
                                continue;

                            u.Remove(u.First(p => p.ID == invitations1[i].ID));
                            invitations1.Remove(invitations1[i]);
                            i--;
                        }
                        if (invitations1.Any() || u.Any())
                        {
                            Invitations.AddRange(u.Select(i => new Invitation
                            {
                                ID = i.ID,
                                TimeSent = i.TimeSent,
                                Sender = i.Sender,
                                Receiver = i.Receiver,
                                Room = i.Room
                            }));


                            foreach (var oldInvite in invitations1)
                            {
                                Invitations.Remove(Invitations.First(s => s.ID == oldInvite.ID));
                            }
                        }
                    };
                    api.Queue(invitations);
                }
            });
            dependencies.Cache(stack);
            content.Add(sideMenu = new SideMenuOverlay());
            dependencies.Cache(sideMenu);
            content.Add(infoOverlay = new SplashInfoOverlay());
            dependencies.Cache(infoOverlay);
            content.Add(new Box
            {
                Colour = Colour4.Black,
                RelativeSizeAxes = Axes.X,
                Height = 100,
                Y = -100,
            });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            //Cargamos asincronamente la pantalla de inicio de sesión y la agregamos al inicio de nuestra pila.
            LoadComponentAsync(startScreen = new SessionStartScreen(), stack.Push);
        }

        private class PaddedDrawSizePreservingFillContainer : DrawSizePreservingFillContainer
        {
            protected override void LoadComplete()
            {
                base.LoadComplete();
                Content.Masking = true;
                Content.Padding = new MarginPadding { Top = 100 };
            }
        }

        public void DownloadGame(OnlineGame game)
        {
            sideMenu.Hide();
            var getGame = new DownloadProjectRequest(game.Id, game.Hash, store);
            getGame.Success += g => importGame(game);
            getGame.Progressed += g =>
            {
                infoOverlay.Show("Descargando juego...", Colour4.Turquoise);
            };
            api.Queue(getGame);
        }

        private void importGame(OnlineGame onlineGame)
        {
            string filename = store.GetFullPath(Path.Combine(@"download", @$"{onlineGame.Hash}.zip"));

            using (var fileStream = store.GetStream(filename, FileAccess.Read, FileMode.Open))
            {
                using (ZipFile zip = ZipFile.Read(fileStream))
                {
                    foreach (ZipEntry e in zip)
                    {
                        e.Extract(store.GetFullPath("files"), ExtractExistingFileAction.DoNotOverwrite);

                        if (e.FileName == Path.GetFileNameWithoutExtension(filename))
                            continue;
                    }
                }
            }
        }

        public void Logout()
        {
            api.Logout();
            startScreen.MakeCurrent();
            sideMenu.Hide();
        }

        protected override bool OnExiting()
        {
            Logout();
            return base.OnExiting();
        }
    }
}
