using GamesToGo.Game.Graphics;
using GamesToGo.Game.Online;
using GamesToGo.Game.Overlays;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osuTK;

namespace GamesToGo.Game.Screens
{
    public class GameInfoScreen : Screen
    {
        [Resolved]
        private SideMenuOverlay sideMenu { get; set; }
        [Resolved]
        private MainMenuScreen mainMenu { get; set; }
        [Resolved]
        private APIController api { get; set; }
        [Resolved]
        private SessionStartScreen startScreen { get; set; }
        private TextFlowContainer errorText;

        private readonly OnlineGame game;
        private FillFlowContainer gameRooms;

        public GameInfoScreen(OnlineGame game)
        {
            this.game = game;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            sideMenu.NextScreen = startScreen.MakeCurrent;
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
                        new Dimension()
                    },
                    ColumnDimensions = new[]
                    {
                        new Dimension()
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
                                            Action = mainMenu.MakeCurrent,
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
                                                    new GamePreviewContainer(game)
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
                                                            Text = game.Description,
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
                            Action = () => createRoom(game),
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
            };
            populateRooms();
        }

        private void populateRooms()
        {
            var getRooms = new GetAllRoomsFromGameRequest(game.Id);
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
            var room = new CreateRoomRequest(requestedGame.Id);
            room.Success += u =>
            {
                LoadComponentAsync(new RoomScreen(u), this.Push);
            };
            api.Queue(room);
        }

        private void joinRoom(int id)
        {
            var room = new JoinRoomRequest(id);
            room.Success += u =>
            {
                LoadComponentAsync(new RoomScreen(u), this.Push);
            };
            room.Failure += e =>
            {
                Logger.Log(e.Message);
            };
            api.Queue(room);
        }
    }
}
