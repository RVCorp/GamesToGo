using System.Linq;
using GamesToGo.Common.Online;
using GamesToGo.Game.Graphics;
using GamesToGo.Game.LocalGame;
using GamesToGo.Game.Online;
using GamesToGo.Game.Online.Models.RequestModel;
using GamesToGo.Game.Online.Requests;
using GamesToGo.Game.Overlays;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;

namespace GamesToGo.Game.Screens
{
    public class RoomScreen : Screen
    {
        [Resolved]
        private APIController api { get; set; }
        [Resolved]
        private Storage store { get; set; }

        [Cached]
        private readonly Bindable<OnlineRoom> room;

        [Cached]
        private WorkingGame game = new WorkingGame();

        [Cached]
        private RoomUpdater roomUpdater = new RoomUpdater
        {
            TimeBetweenPolls = 1000,
        };

        private FillFlowContainer usersInRoom;
        private SpriteText textButton;
        private readonly InvitePlayersToRoomOverlay inviteOverlay = new InvitePlayersToRoomOverlay();
        private ScreenStack gameStack;

        private DependencyContainer dependencies;
        private SurfaceButton continueButton;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent) =>
            dependencies = new DependencyContainer(base.CreateChildDependencies(parent));

        public RoomScreen(OnlineRoom room)
        {
            this.room = new Bindable<OnlineRoom>(room);
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            game.Parse(store, textures, room.Value.Game);

            dependencies.Cache(room.Value.Players.Single(p => p?.BackingUser?.ID == api.LocalUser.Value.ID));

            RelativeSizeAxes = Axes.Both;

            InternalChildren = new Drawable[]
            {
                roomUpdater,
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
                                        Children = new Drawable[]
                                        {
                                            new SimpleIconButton(FontAwesome.Solid.SignOutAlt)
                                            {
                                                Anchor = Anchor.Centre,
                                                Origin = Anchor.Centre,
                                                Action = exitRoom,
                                            },
                                        }
                                    },
                                    new Container
                                    {
                                        Anchor = Anchor.TopRight,
                                        Origin = Anchor.TopRight,
                                        RelativeSizeAxes = Axes.Both,
                                        Width = .2f,
                                        Children = new Drawable[]
                                        {
                                            new SimpleIconButton(FontAwesome.Solid.Envelope)
                                            {
                                                Anchor = Anchor.Centre,
                                                Origin = Anchor.Centre,
                                                Action = inviteOverlay.Show,
                                            },
                                        }
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
                                                        Text = @"Jugadores:",
                                                        Font = new FontUsage(size: 80)
                                                    },
                                                    new BasicScrollContainer
                                                    {
                                                        RelativeSizeAxes = Axes.X,
                                                        Height = 1000,
                                                        ClampExtension = 30,
                                                        Masking = true,
                                                        Child = usersInRoom = new FillFlowContainer
                                                        {
                                                            RelativeSizeAxes = Axes.X,
                                                            AutoSizeAxes = Axes.Y,
                                                            Direction = FillDirection.Full,
                                                            Spacing = new Vector2(0, 20),
                                                        },
                                                    },
                                                },
                                            },
                                        },
                                        new Container
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            Height = .15f,
                                            Child = continueButton = new SurfaceButton
                                            {
                                                Action = playGame,
                                                BackgroundColour = Colour4.LightPink,
                                                Children = new Drawable[]
                                                {
                                                    textButton = new SpriteText
                                                    {
                                                        Anchor = Anchor.Centre,
                                                        Origin = Anchor.Centre,
                                                        Font = new FontUsage(size: 80),
                                                        Colour = new Colour4(106, 100, 104, 255)
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
                inviteOverlay,
                gameStack = new ScreenStack
                {
                    RelativeSizeAxes = Axes.Both,
                },
            };

            Schedule(() => LoadComponentAsync(new GameScreen(), gameStack.Push));

            if (api.LocalUser.Value.ID == room.Value.Owner.BackingUser.ID)
                textButton.Text = "Jugar!";
            else
                textButton.Text = "Listo!";

            for (int i = 0; i < room.Value.Game.Maxplayers; i++)
            {
                usersInRoom.Add(new PlayerInfoContainer(i));
            }

            room.BindValueChanged(roomUpdate => Refresh(roomUpdate.NewValue), true);
            gameStack.Hide();
        }

        private void exitRoom()
        {
            var req = new ExitRoomRequest();
            req.Success += this.Exit;
            api.Queue(req);
        }

        private void playGame()
        {
            continueButton.BackgroundColour = Colour4.DeepPink;
            var req = new ReadyRequest();
            api.Queue(req);
        }

        private void Refresh(OnlineRoom updatedRoom)
        {

            if (updatedRoom.HasStarted)
            {
                gameStack.Show();
                roomUpdater.TimeBetweenPolls = 500;
            }
        }

        private class PlayerInfoContainer : Container
        {
            private readonly int index;
            private SpriteText playerName;

            private static readonly Colour4 ready_green = Colour4.FromHex("8CDD81");
            private static readonly Colour4 waiting_red = Colour4.FromHex("E9967A");
            private static readonly Colour4 empty_gray = Colour4.Gray;
            private static readonly Colour4 icon_green = Colour4.FromHex("4AC948");
            private Box colourBox;

            [Resolved]
            private Bindable<OnlineRoom> onlineRoom { get; set; }

            public PlayerInfoContainer(int index)
            {
                this.index = index;
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                RelativeSizeAxes = Axes.X;
                Width = 0.5f;
                Height = 100;
                Padding = new MarginPadding { Horizontal = 10f };

                Child = new Container
                {
                    Masking = true,
                    CornerRadius = 15f,
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        colourBox = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = empty_gray,
                        },
                        playerName = new SpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Font = new FontUsage(size: 50),
                        },
                    },
                };

                onlineRoom.BindValueChanged(updateDisplay, true);
            }

            private void updateDisplay(ValueChangedEvent<OnlineRoom> roomChange)
            {
                playerName.Text = roomChange.NewValue.Players[index]?.BackingUser.Username ?? @"Esperando Jugador...";

                Colour4 targetBackground;

                if (roomChange.NewValue.Players[index] == null)
                    targetBackground = empty_gray;
                else if (roomChange.NewValue.Players[index].Ready)
                    targetBackground = ready_green;
                else
                    targetBackground = waiting_red;

                colourBox.FadeColour(targetBackground, 200, Easing.OutQuart);
            }
        }
    }
}
