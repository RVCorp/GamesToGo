using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GamesToGo.Game.Graphics;
using GamesToGo.Game.LocalGame;
using GamesToGo.Game.LocalGame.Elements;
using GamesToGo.Game.Online;
using GamesToGo.Game.Online.Models.OnlineProjectElements;
using GamesToGo.Game.Online.Models.RequestModel;
using GamesToGo.Game.Online.Requests;
using osu.Framework.Allocation;
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
    public class GameScreen : Screen
    {
        private OnlineRoom room;
        private FillFlowContainer playersImages;
        private TextureStore textures;
        private CancellationTokenSource _tokenSource;
        private FillFlowContainer<CardContainer> playerCards;
        private Player player;
        [Cached]
        private WorkingGame file = new WorkingGame();
        private BoardsContainer board;

        [Resolved]
        private APIController api { get; set; }
        [Resolved]
        private Storage store { get; set; }

        public GameScreen(OnlineRoom room)
        {
            this.room = room;
        }

        public CancellationToken Token { get; private set; }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            this.textures = textures;
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
                                    Width = .9f,
                                    ScrollbarOverlapsContent = false,
                                    Child = playersImages = new FillFlowContainer
                                    {
                                        RelativeSizeAxes = Axes.Y,
                                        AutoSizeAxes = Axes.X,
                                        Direction = FillDirection.Horizontal
                                    }
                                },
                                new SimpleIconButton(FontAwesome.Solid.SignOutAlt)
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Width = .1f,
                                }
                            }
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
                                        Direction = FillDirection.Horizontal
                                    }
                                },
                            }
                        }
                    }
                }
            };            
            populateGame();
            player = room.Players.First(p => p.BackingUser.ID == api.LocalUser.Value.ID);
            _tokenSource = new CancellationTokenSource();
            var token = _tokenSource.Token;
            Token = token;
            Task.Run(() =>
            { 
                while (!room.HasStarted)
                {
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                    System.Threading.Thread.Sleep(1000);
                    var roomStateRequest = new GetRoomStateRequest();
                    roomStateRequest.Success += u =>
                    {
                        refreshRoom(u);
                    };
                    api.Queue(roomStateRequest);
                }
            });
        }

        private void refreshRoom(OnlineRoom room)
        {
            if (room != null)
            {
                populatePlayers(room);
                if (room.Players.First(p => p.BackingUser.ID == player.BackingUser.ID).Hand.Cards != null)
                {
                    checkPlayerHand(room.Players.First(p => p.BackingUser.ID == player.BackingUser.ID).Hand.Cards);
                    foreach(var card in player.Hand.Cards)
                    {
                        //playerCards.Add(); // Hacer CardContainer
                    }
                }
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

        private void checkPlayerHand(List<OnlineCard> cards)
        {
            List<OnlineCard> cards1 = player.Hand.Cards;
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
                player.Hand.Cards.AddRange(cards.Select(c => new OnlineCard
                {
                    ID = c.ID,
                    TypeID = c.TypeID,
                    Orientation = c.Orientation,
                    FrontVisible = c.FrontVisible,
                    Tokens = c.Tokens
                }));

                foreach(var card in player.Hand.Cards)
                {
                    playerCards.Add(new CardContainer(card));
                }

                foreach (var oldCard in cards1)
                {
                    player.Hand.Cards.Remove(player.Hand.Cards.First(s => s.ID == oldCard.ID));
                    playerCards.RemoveRange(playerCards.Where(c => c.Card.ID == oldCard.ID));
                }

            }
        }

        //Start the GameScreen 
        private void populatePlayers(OnlineRoom room)
        {
            foreach(var player in room.Players)
            {
                if(player != null)
                {
                    playersImages.Add(new Container
                    {
                        RelativeSizeAxes = Axes.Y,
                        Width = 180,
                        Child = new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Direction = FillDirection.Vertical,
                            Children = new Drawable[]
                            {
                                new CircularContainer
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    BorderColour = Colour4.Black,
                                    BorderThickness = 3.5f,
                                    Masking = true,
                                    Size = new Vector2(100),
                                    Child = new Sprite
                                    {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        RelativeSizeAxes = Axes.Both,
                                        Texture = textures.Get($"https://gamestogo.company/api/Users/DownloadImage/{player.BackingUser.ID}")
                                    }
                                },
                                new SpriteText
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Text = player.BackingUser.Username,
                                    Font = new FontUsage(size: 40)
                                },
                            }
                        }
                    });
                }
            }
        }

        private void populatePlayerHand()
        {

        }        

        private void populateGame()
        {            
            file.Parse(store, textures, room.Game);
            board.Boards = file.GameBoards.ToList();            
            board.PopulateBoards();
        }
    }
}
//En caso de varios tableros, cual mostrar? Debo tener ProjectElement locales? Como muestro el tablero con
