using System;
using System.Collections.Generic;
using System.Linq;
using GamesToGo.Common.Game;
using GamesToGo.Common.Online;
using GamesToGo.Common.Overlays;
using GamesToGo.Game.Graphics;
using GamesToGo.Game.LocalGame;
using GamesToGo.Game.LocalGame.Arguments;
using GamesToGo.Game.LocalGame.Elements;
using GamesToGo.Game.Online.Models.OnlineProjectElements;
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

namespace GamesToGo.Game.Screens
{
    [Cached]
    public class GameScreen : Screen
    {
        private PlayerHandContainer playerCards;
        [Resolved]
        private Player localPlayer { get; set; }
        private BoardsContainer board;

        public Bindable<bool> EnableCardSelection = new BindableBool(false);
        public Bindable<bool> EnableTileSelection = new BindableBool(false);
        public Bindable<bool> EnablePlayerSelection = new BindableBool(false);

        private readonly Bindable<OnlineCard> currentSelectedCard = new Bindable<OnlineCard>();
        public IBindable<OnlineCard> CurrentSelectedCard => currentSelectedCard;

        private readonly Bindable<OnlineTile> currentSelectedTile = new Bindable<OnlineTile>();
        public IBindable<OnlineTile> CurrentSelectedTile => currentSelectedTile;

        private readonly Bindable<Player> currentSelectedPlayer = new Bindable<Player>();
        public IBindable<Player> CurrentSelectedPlayer => currentSelectedPlayer;

        private ArgumentParameter argumentToSend;

        private PlayerPreviewContainer players;

        private Player[] playersArray;

        private int indexOfPlayer = 0;
        private SendArgumentOverlay sendOverlay;

        [Resolved]
        private APIController api { get; set; }
        [Resolved]
        private Storage store { get; set; }
        [Resolved]
        private TextureStore textures { get; set; }

        [Resolved]
        private MainMenuScreen mainMenu { get; set; }

        [Resolved]
        private Bindable<OnlineRoom> room { get; set; }
        [Resolved]
        private WorkingGame game { get; set; }
        [Resolved]
        private SplashInfoOverlay infoOverlay { get; set; }


        [BackgroundDependencyLoader]
        private void load()
        {
            EnableCardSelection.Value = false;
            EnableTileSelection.Value = false;
            EnablePlayerSelection.Value = false;

            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Black
                },
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Height = .1f,
                            Direction = FillDirection.Horizontal,
                            Children = new Drawable[]
                            {
                                new BasicScrollContainer(Direction.Horizontal)
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Width = .8f,
                                    ScrollbarOverlapsContent = false,
                                    Child = players = new PlayerPreviewContainer(),
                                },
                                new SimpleIconButton(FontAwesome.Solid.SignOutAlt)
                                {
                                    Action = exitRoom,
                                },
                            },
                        },
                        board = new BoardsContainer
                        {
                            Height = .6f,
                            Boards = game.GameBoards.ToList()
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Height = .3f,
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = Colour4.Bisque
                                },
                                playerCards = new PlayerHandContainer()
                            },
                        },
                    },
                },
                sendOverlay = new SendArgumentOverlay()
            };
            
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            room.BindValueChanged(updatedRoom => checkRoom(updatedRoom.NewValue), true);
        }
        public void SelectCard(OnlineCard card)
        {
            if (Equals(currentSelectedCard.Value, card))
                currentSelectedCard.Value = null;
            else
                currentSelectedCard.Value = card;
        }

        public void SelectTile(OnlineTile tile)
        {
            currentSelectedTile.Value = currentSelectedTile.Value == tile ? null : tile;
        }

        public void SelectPlayer(Player player)
        {
            currentSelectedPlayer.Value = Equals(currentSelectedPlayer.Value, player) ? null : player;
        }

        private void exitRoom()
        {
            var req = new ExitRoomRequest();
            req.Success += () =>
            {
                mainMenu.MakeCurrent();
                this.Exit();
            };
            api.Queue(req);
        }

        public void Send()
        {
            int id = 0;
            if(EnableCardSelection.Value)
            {
                id = CurrentSelectedCard.Value.ID;
                EnableCardSelection.Value = false;
                currentSelectedCard.Value = null;
            }
            else if(EnableTileSelection.Value)
            {
                if(argumentToSend.Type == ArgumentType.TileWithNoCardsChosenByPlayer)
                {
                    if (CurrentSelectedTile.Value.Cards.Count == 0)
                    {
                        id = CurrentSelectedTile.Value.TypeID;
                        EnableTileSelection.Value = false;
                        currentSelectedTile.Value = null;
                    }
                    else
                    {
                        EnableTileSelection.Value = false;
                        currentSelectedTile.Value = null;
                        Schedule(() => infoOverlay.Show(@"Selecciona una casilla que no tenga cartas", Colour4.DarkRed));                        
                    }
                }
                else
                {
                    id = CurrentSelectedTile.Value.TypeID;
                    EnableTileSelection.Value = false;
                    currentSelectedTile.Value = null;
                }
            }
            else if (EnablePlayerSelection.Value)
            {
                id = Array.IndexOf<Player>(playersArray, CurrentSelectedPlayer.Value);
                EnablePlayerSelection.Value = false;
                currentSelectedPlayer.Value = null;
            }

            if(id != 0)
            {
                var SendArgument = new SendArgumentRequest(id);
                SendArgument.Failure += e =>
                {
                    Console.WriteLine(@"No se pudo enviar el argumento");
                };
                api.Queue(SendArgument);
            }
        }

        private void checkRoom(OnlineRoom receivedRoom)
        {
            checkActions(receivedRoom);
            checkVictory(receivedRoom);
        }

        private void checkVictory(OnlineRoom receivedRoom)
        {
            if (receivedRoom.WinnerPlayerIndexes == null)
                return;

            if(receivedRoom.WinnerPlayerIndexes.Count == 1)
            {
                if (localPlayer.BackingUser.ID == receivedRoom.Players[receivedRoom.WinnerPlayerIndexes.FirstOrDefault()].BackingUser.ID)
                    Schedule(() => infoOverlay.Show(@"Felicidades, ganaste!", Colour4.Green));
                else
                    Schedule(() => infoOverlay.Show(@"Suerte para la proxima, perdiste!", Colour4.DarkRed));
            }
            else if(receivedRoom.WinnerPlayerIndexes.Count > 1)
                Schedule(() => infoOverlay.Show(@"Empate!", Colour4.Blue));
            else if(receivedRoom.WinnerPlayerIndexes.Count == 0)
                Schedule(() => infoOverlay.Show(@"Nadie ganó!", Colour4.DarkRed));
        }

        private void checkActions(OnlineRoom receivedRoom)
        {
            if (receivedRoom.UserActionArgument != null)
            {
                argumentToSend = receivedRoom.UserActionArgument;
                switch (receivedRoom.UserActionArgument.Type)
                {
                    case ArgumentType.TileWithNoCardsChosenByPlayer:
                    {
                        argumentWithOneArgument(receivedRoom, ArgumentType.TileWithNoCardsChosenByPlayer.ReturnType());
                    }
                    break;
                    case ArgumentType.PlayerChosenByPlayer:
                    {
                        argumentWithOneArgument(receivedRoom, ArgumentType.PlayerChosenByPlayer.ReturnType());
                    }
                    break;
                    case ArgumentType.DefaultArgument: //TileWithTokenSelectedByPlayer 
                    {

                    }
                    break;
                }
            }
        }

        private void argumentWithOneArgument(OnlineRoom receivedRoom, ArgumentReturnType argumentType)
        {
            if (receivedRoom.Players[receivedRoom.UserActionArgument.Arguments[0].Result[0]].BackingUser.ID == localPlayer.BackingUser.ID)
            {
                if((argumentType & ArgumentReturnType.Tile) != 0)
                {
                    EnableTileSelection.Value = true;
                    sendOverlay.Show();
                }
                if ((argumentType & ArgumentReturnType.Card) != 0)
                {
                    EnableCardSelection.Value = true;
                    sendOverlay.Show();
                }
                if ((argumentType & ArgumentReturnType.Player) != 0)
                {
                    EnablePlayerSelection.Value = true;
                    sendOverlay.Show();
                    playersArray = receivedRoom.Players;
                }
            }
        }

        private void argumentWithManyArguments(OnlineRoom receivedRoom, ArgumentType argumentType)
        {
            for (int i = 0; i < receivedRoom.UserActionArgument.Arguments.Count; i++)
            {
                if(receivedRoom.UserActionArgument.Arguments[i].Type.ReturnType() == ArgumentReturnType.SinglePlayer)
                {

                }
            }
        }
    }
}
