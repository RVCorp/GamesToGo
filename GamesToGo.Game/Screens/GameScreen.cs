using System;
using System.Collections.Generic;
using System.Linq;
using GamesToGo.Common.Game;
using GamesToGo.Common.Online;
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

        public OnlineCard CurrentCard = new OnlineCard();
        public OnlineTile CurrentTile = new OnlineTile();
        public Player CurrentPlayer = new Player();
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
            room.BindValueChanged(updatedRoom => checkActions(updatedRoom.NewValue), true);
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
            if(CurrentTile == null && CurrentPlayer == null)
            {
                CurrentCard = playerCards.CurrentSelectedCard.Value;
                id = CurrentCard.ID;
                EnableCardSelection.Value = false;
            }
            else if(CurrentCard == null && CurrentPlayer == null)
            {
                CurrentTile = board.CurrentSelectedTile.Value;
                id = CurrentTile.TypeID;
                EnableTileSelection.Value = false;
            }
            else if (CurrentTile == null && CurrentCard == null)
            {
                CurrentPlayer = players.CurrentSelectedPlayer.Value;
                id = Array.IndexOf<Player>(playersArray, CurrentPlayer);
                EnablePlayerSelection.Value = false;
            }

            var SendArgument = new JoinRoomRequest(id);
            SendArgument.Failure += e =>
            {
                Console.WriteLine( @"No se pudo enviar el argumento");
            };
            api.Queue(SendArgument);
        }

        private void checkActions(OnlineRoom receivedRoom)
        {
            if (receivedRoom.UserActionArgument != null)
            {
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

        private void argumentWithOneArgument(OnlineRoom receivedRoom, ArgumentReturnType argument)
        {
            if (receivedRoom.Players[receivedRoom.UserActionArgument.Arguments[0].Result[0]].BackingUser.ID == localPlayer.BackingUser.ID)
            {
                if((argument & ArgumentReturnType.Tile) != 0)
                {
                    EnableTileSelection.Value = true;
                    sendOverlay.Show();
                    CurrentCard = null;
                    CurrentPlayer = null;
                }
                if ((argument & ArgumentReturnType.Card) != 0)
                {
                    EnableCardSelection.Value = true;
                    sendOverlay.Show();
                    CurrentTile = null;
                    CurrentPlayer = null;
                }
                if ((argument & ArgumentReturnType.Player) != 0)
                {
                    EnablePlayerSelection.Value = true;
                    sendOverlay.Show();
                    CurrentTile = null;
                    CurrentCard = null;
                    playersArray = receivedRoom.Players;
                }
            }
        }
    }
}
