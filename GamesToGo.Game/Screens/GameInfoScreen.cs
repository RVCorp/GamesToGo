using GamesToGo.Common.Online;
using GamesToGo.Common.Online.RequestModel;
using GamesToGo.Common.Overlays;
using GamesToGo.Game.Graphics;
using GamesToGo.Game.Online.Requests;
using GamesToGo.Game.Overlays;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using osuTK;

namespace GamesToGo.Game.Screens
{
    public class GameInfoScreen : Screen
    {
        [Resolved]
        private GamesToGoGame game { get; set; }
        [Resolved]
        private SideMenuOverlay sideMenu { get; set; }
        [Resolved]
        private MainMenuScreen mainMenu { get; set; }
        [Resolved]
        private APIController api { get; set; }
        [Resolved]
        private SplashInfoOverlay infoOverlay { get; set; }

        private TextFlowContainer errorText;

        private readonly OnlineGame onlineGame;
        private FillFlowContainer gameRooms;
        private GamesToGoButton reportButton;
        private ReportOverlay reportOverlay;

        public GameInfoScreen(OnlineGame game)
        {
            onlineGame = game;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Colour4(106, 100, 104, 255)
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    RowDimensions = new[]
                    {
                        new Dimension(GridSizeMode.Relative, .1f),
                        new Dimension(),
                    },
                    ColumnDimensions = new[]
                    {
                        new Dimension(),
                    },
                    Content = new[]
                    {
                        new Drawable[]
                        {
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = new Colour4(60, 60, 60, 255)
                                    },
                                    new Container
                                    {
                                        Anchor = Anchor.TopLeft,
                                        Origin = Anchor.TopLeft,
                                        RelativeSizeAxes = Axes.Both,
                                        Width = .2f,
                                        Child = new SimpleIconButton(FontAwesome.Solid.Bars)
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Action = () => sideMenu.Show()
                                        }
                                    },
                                    new Container
                                    {
                                        Anchor = Anchor.TopRight,
                                        Origin = Anchor.TopRight,
                                        RelativeSizeAxes = Axes.Both,
                                        Width = .2f,
                                        Child = new SimpleIconButton(FontAwesome.Solid.Home)
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Action = this.Exit,
                                        }
                                    },
                                }
                            }
                        },
                        new Drawable[]
                        {
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Child = new BasicScrollContainer
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    ClampExtension = 30,
                                    Child = new FillFlowContainer
                                    {
                                        AutoSizeAxes = Axes.Y,
                                        RelativeSizeAxes = Axes.X,
                                        Direction = FillDirection.Vertical,
                                        Children = new Drawable[]
                                        {
                                            new Container
                                            {
                                                RelativeSizeAxes = Axes.X,
                                                Height = 400,
                                                Children = new Drawable[]
                                                {
                                                    new Box
                                                    {
                                                        RelativeSizeAxes = Axes.Both,
                                                        Colour = Colour4.Red
                                                    },
                                                    new GamePreviewContainer(onlineGame)
                                                    {
                                                        GameNameSize = 90,
                                                        MadeBySize = 60,
                                                    }
                                                }
                                            },
                                            new Container
                                            {
                                                RelativeSizeAxes = Axes.X,
                                                AutoSizeAxes = Axes.Y,
                                                Child = new FillFlowContainer
                                                {
                                                    RelativeSizeAxes = Axes.X,
                                                    AutoSizeAxes = Axes.Y,
                                                    Direction = FillDirection.Vertical,
                                                    Children = new Drawable[]
                                                    {
                                                        new SpriteText
                                                        {
                                                            Text = @"Descripción:",
                                                            Font = new FontUsage(size: 70)
                                                        },
                                                        new TextFlowContainer((e) => e.Font = new FontUsage(size: 70) )
                                                        {
                                                            RelativeSizeAxes = Axes.X,
                                                            Height = 300,
                                                            Text = onlineGame.Description,
                                                        },
                                                    },
                                                },
                                            },
                                            new SpriteText
                                            {
                                                Text = "Salas:",
                                                Font = new FontUsage(size: 80)
                                            },
                                            new Container
                                            {
                                                RelativeSizeAxes = Axes.X,
                                                AutoSizeAxes = Axes.Y,
                                                Padding = new MarginPadding(80),
                                                Children = new Drawable[]
                                                {
                                                    new Container
                                                    {
                                                        RelativeSizeAxes = Axes.Both,
                                                        Padding = new MarginPadding(.9f),
                                                        Children = new Drawable[]
                                                        {
                                                            new Box
                                                            {
                                                                RelativeSizeAxes = Axes.Both,
                                                                Colour = Colour4.LightGray
                                                            },
                                                            errorText = new TextFlowContainer((e) => { e.Font = new FontUsage(size: 80); e.Colour = Colour4.Black; })
                                                            {
                                                                RelativeSizeAxes = Axes.X,
                                                                Height = 1000,
                                                            },
                                                        },
                                                    },
                                                    new BasicScrollContainer
                                                    {
                                                        RelativeSizeAxes = Axes.X,
                                                        Height = 1000,
                                                        ClampExtension = 30,
                                                        Masking = true,
                                                        BorderColour = Colour4.Black,
                                                        BorderThickness = 3.5f,
                                                        Child = gameRooms = new FillFlowContainer
                                                        {
                                                            RelativeSizeAxes = Axes.X,
                                                            AutoSizeAxes = Axes.Y,
                                                            Direction = FillDirection.Vertical
                                                        },
                                                    },
                                                },
                                            },
                                            new Container
                                            {
                                                RelativeSizeAxes = Axes.X,
                                                Height = 200,
                                                Child = reportButton = new GamesToGoButton
                                                {
                                                    Anchor = Anchor.Centre,
                                                    Origin = Anchor.Centre,
                                                    RelativeSizeAxes = Axes.X,
                                                    Height = 150,
                                                    Width = .85f,
                                                    Text = @"Reportar",
                                                    Action = () => reportOverlay.Show()
                                                },
                                            },
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
                new Container
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    Size = new Vector2(300),
                    Padding = new MarginPadding(50),
                    Child = new CircularContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        Masking = true,
                        Child = new SurfaceButton
                        {
                            Action = () => createRoom(onlineGame),
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = Colour4.Pink
                                },
                                new SpriteIcon
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    RelativeSizeAxes = Axes.Both,
                                    Height = .4f,
                                    Width = .4f,
                                    Icon = FontAwesome.Solid.Plus,
                                },
                            },
                        },
                    },
                },
                reportOverlay = new ReportOverlay()
                {
                    Game = onlineGame
                }
            };
            reportButton.SpriteText.Font = new FontUsage(size: 80);
            populateRooms();
        }

        private void populateRooms()
        {
            var getRooms = new GetAllRoomsFromGameRequest(onlineGame.Id);
            getRooms.Success += u =>
            {
                foreach(var room in u)
                {
                    gameRooms.Add(new Container
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 200,
                        Child = new SurfaceButton
                        {
                            Child = new RoomPreviewContainer(room),
                            Action = () => joinRoom(room.Id)
                        }
                    });
                }
            };
            getRooms.Failure += e =>
            {
                errorText.Text = @"No hay salas disponibles para este juego";
            };
            api.Queue(getRooms);
        }

        private void createRoom(OnlineGame requestedGame)
        {
            game.DownloadGame(requestedGame);
            var room = new CreateRoomRequest(requestedGame.Id);
            room.Success += u =>
            {
                LoadComponentAsync(new RoomScreen(u), this.Push);
            };

            room.Failure += ex =>
            {
                infoOverlay.Show(@"Ocurrió un error al intentar crear la sala", Colour4.DarkRed);
            };
            api.Queue(room);
        }

        private void joinRoom(int id)
        {
            game.DownloadGame(onlineGame);
            var joinRoom = new JoinRoomRequest(id);
            joinRoom.Success += u =>
            {
                LoadComponentAsync(new RoomScreen(u), this.Push);
            };

            joinRoom.Failure += _ => infoOverlay.Show(@"Hubo un problema al intentar entrar en la sala", Colour4.DarkRed);
            api.Queue(joinRoom);
        }
    }
}
