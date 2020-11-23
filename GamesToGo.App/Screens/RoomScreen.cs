using System.Threading.Tasks;
using GamesToGo.App.Graphics;
using GamesToGo.App.Online;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;

namespace GamesToGo.App.Screens
{
    public class RoomScreen : Screen
    {
        [Resolved]
        private MainMenuScreen mainMenu { get; set; }
        [Resolved]
        private APIController api { get; set; }
        private OnlineRoom room;
        private FillFlowContainer<TextContainer> usersInRoom;
        private SpriteText TextButton;

        public RoomScreen(OnlineRoom room)
        {
            this.room = room;
        }

        [BackgroundDependencyLoader]
        private void load()
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
                                        Child = new SimpleIconButton(FontAwesome.Solid.SignOutAlt)
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Action = mainMenu.MakeCurrent
                                        },
                                    },
                                },
                            },
                        },
                        new Drawable[]
                        {
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Child = new FillFlowContainer
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Direction = FillDirection.Vertical,
                                    Padding = new MarginPadding(50),
                                    Children = new Drawable[]
                                    {
                                        new Container
                                        {
                                            RelativeSizeAxes = Axes.X,
                                            Height = 1000,
                                            Child = new FillFlowContainer
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                Direction = FillDirection.Vertical,
                                                Children = new Drawable[]
                                                {
                                                    new SpriteText
                                                    {
                                                        Text = "Jugadores:",
                                                        Font = new FontUsage(size: 80)
                                                    },
                                                    new BasicScrollContainer
                                                    {
                                                        RelativeSizeAxes = Axes.X,
                                                        Height = 1000,
                                                        ClampExtension = 30,
                                                        Masking = true,
                                                        Child = usersInRoom = new FillFlowContainer<TextContainer>
                                                        {
                                                            RelativeSizeAxes = Axes.Both,
                                                            Direction = FillDirection.Full,
                                                        }
                                                    }
                                                }
                                            }
                                        },
                                        new Container
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            Height = .15f,
                                            Child = new SurfaceButton
                                            {
                                                Children = new Drawable[]
                                                {
                                                    new Box
                                                    {
                                                        RelativeSizeAxes = Axes.Both,
                                                        Colour = Colour4.LightPink
                                                    },
                                                    TextButton =new SpriteText
                                                    {
                                                        Anchor = Anchor.Centre,
                                                        Origin = Anchor.Centre,
                                                        Font = new FontUsage(size: 80),
                                                        Colour = new Colour4(106, 100, 104, 255)
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
            };
            if (api.LocalUser.Value.ID == room.Owner.BackingUser.ID)
                TextButton.Text = "Jugar!";
            else
                TextButton.Text = "Listo!";
            populateUsersList();
            Task RefreshRoom = Task.Run(() =>
            {
                while (!room.HasStarted)
                {
                    System.Threading.Thread.Sleep(10000);
                    var room = new GetRoomStateRequest();
                    room.Success += u =>
                    {
                        Refresh(u);
                    };
                    api.Queue(room);
                }
            });
        }

        private void populateUsersList()
        {
            for (int i = 0; i < room.Game.Maxplayers; i++)
            {
                    usersInRoom.Add(new TextContainer());
            }
            Refresh(room);
        }

        private void Refresh(OnlineRoom room)
        {
            this.room = room;
            for (int i = 0; i < room.Game.Maxplayers; i++)
            {
                if(room.Players[i] != null)
                {
                    usersInRoom[i].Text.Text = room.Players[i].BackingUser.Username;
                }
            }
        }

        private class TextContainer : Container
        {
            public SpriteText Text;

            [BackgroundDependencyLoader]
            private void load()
            {
                RelativeSizeAxes = Axes.X;
                Width = .5f;
                Height = 100;
                Child = Text = new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Font = new FontUsage(size: 50)
                };
            }
        }
    }
}
