using GamesToGo.App.Graphics;
using GamesToGo.App.Online;
using GamesToGo.App.Overlays;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osuTK;

namespace GamesToGo.App.Screens
{
    public class GameInfoScreen : Screen
    {
        [Resolved]
        private DropdownMenuOverlay dropdownMenu { get; set; }
        [Resolved]
        private MainMenuScreen ms { get; set; }
        [Resolved]
        private APIController api { get; set; }
        private TextFlowContainer ErrorText;

        private OnlineGame game;
        private FillFlowContainer GameRooms;

        public GameInfoScreen(OnlineGame game)
        {
            this.game = game;
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
                                            Action = () => dropdownMenu.Show()
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
                                            Action = goHome
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
                                                            Text = "Descripción:",
                                                            Font = new FontUsage(size: 70)
                                                        },
                                                        new TextFlowContainer((e) => e.Font = new FontUsage(size: 70) )
                                                        {
                                                            RelativeSizeAxes = Axes.X,
                                                            Height = 300,
                                                            Text = game.Description,
                                                        },
                                                    }
                                                }
                                            },
                                            new Container
                                            {
                                                RelativeSizeAxes = Axes.X,
                                                AutoSizeAxes = Axes.Y,
                                                Padding = new MarginPadding(80),
                                                Children = new Drawable[]
                                                {
                                                    new BasicScrollContainer
                                                    {
                                                        RelativeSizeAxes = Axes.X,
                                                        Height = 1000,
                                                        ClampExtension = 30,
                                                        Masking = true,
                                                        BorderColour = Colour4.Black,
                                                        BorderThickness = 3.5f,
                                                        Child = GameRooms = new FillFlowContainer
                                                        {
                                                            RelativeSizeAxes = Axes.X,
                                                            AutoSizeAxes = Axes.Y,
                                                            Direction = FillDirection.Vertical
                                                        },
                                                    },
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
                                                            ErrorText = new TextFlowContainer((e) => { e.Font = new FontUsage(size: 80); e.Colour = Colour4.Black; })
                                                            {
                                                                RelativeSizeAxes = Axes.X,
                                                                Height = 1000,
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                dropdownMenu = new DropdownMenuOverlay()
                {

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
                            new SurfaceButton { Action = () => createRoom(game)}
                        }
                    }
                }
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
                    GameRooms.Add(new Container
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 200,
                        Masking = true,
                        BorderColour = Colour4.Black,
                        BorderThickness = 3.5f,
                        Children = new Drawable[]
                        {
                            new RoomPreviewContainer(game, room),
                            new SurfaceButton { }
                        }
                    });
                }
            };
            getRooms.Failure += e =>
            {
                ErrorText.Text = "No hay salas disponibles para este juego";
            };
            api.Queue(getRooms);
        }

        private void goHome()
        {
            LoadComponentAsync(ms, this.Push);
        }

        private void createRoom(OnlineGame game)
        {
            var room = new CreateRoomRequest(game.Id);
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
