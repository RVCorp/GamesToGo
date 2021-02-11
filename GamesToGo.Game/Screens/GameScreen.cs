using System.Collections.Generic;
using System.Linq;
using GamesToGo.Common.Online;
using GamesToGo.Game.Graphics;
using GamesToGo.Game.Online.Models.OnlineProjectElements;
using GamesToGo.Game.Online.Models.RequestModel;
using GamesToGo.Game.Online.Requests;
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
    public class GameScreen : Screen
    {
        private FillFlowContainer<CardContainer> playerCards;
        [Resolved]
        private Player localPlayer { get; set; }
        private BoardsContainer board;

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

        [BackgroundDependencyLoader]
        private void load()
        {
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
                                    Child = new PlayerPreviewContainer(),
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
                                new BasicScrollContainer(Direction.Horizontal)
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    ScrollbarOverlapsContent = false,
                                    Child = playerCards = new FillFlowContainer<CardContainer>
                                    {
                                        RelativeSizeAxes = Axes.Y,
                                        AutoSizeAxes = Axes.X,
                                        Direction = FillDirection.Horizontal,
                                    },
                                },
                            },
                        },
                    },
                },
            };
            room.BindValueChanged(updatedRoom => refreshRoom(updatedRoom.NewValue), true);
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

        private void refreshRoom(OnlineRoom receivedRoom)
        {
            if (receivedRoom == null)
                return;

            checkPlayerHand(receivedRoom.PlayerWithID(api.LocalUser.Value.ID).Hand.Cards);

            foreach (var card in localPlayer.Hand.Cards)
            {
                //playerCards.Add(); // Hacer CardContainer
            }
        }

        private void checkPlayer()
        {
            //Hand, Status...
        }

        private void checkObjects()
        {
            //Board, Tile, Cards, Tokens..
        }

        private void checkPlayerHand(ICollection<OnlineCard> cards)
        {
            List<OnlineCard> cards1 = localPlayer.Hand.Cards;
            for (int i = 0; i < cards1.Count; i++)
            {
                if (cards.All(c => c.ID != cards1[i].ID))
                    continue;

                cards.Remove(cards.First(c => c.ID == cards1[i].ID));
                cards1.Remove(cards1[i]);
                i--;
            }
            if (cards1.Any() || cards.Any())
            {
                localPlayer.Hand.Cards.AddRange(cards.Select(c => new OnlineCard
                {
                    ID = c.ID,
                    TypeID = c.TypeID,
                    Orientation = c.Orientation,
                    FrontVisible = c.FrontVisible,
                    Tokens = c.Tokens
                }));

                foreach(var card in localPlayer.Hand.Cards)
                {
                    playerCards.Add(new CardContainer(card));
                }

                foreach (var oldCard in cards1)
                {
                    localPlayer.Hand.Cards.Remove(localPlayer.Hand.Cards.First(s => s.ID == oldCard.ID));
                    playerCards.RemoveRange(playerCards.Where(c => c.Card.ID == oldCard.ID));
                }

            }
        }

        //Start the GameScreen
        private void populatePlayers()
        {

        }

        private void populatePlayerHand()
        {

        }
    }
}
//En caso de varios tableros, cual mostrar? Debo tener ProjectElement locales? Como muestro el tablero con
