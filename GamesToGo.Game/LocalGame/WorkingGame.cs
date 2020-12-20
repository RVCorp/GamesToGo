using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GamesToGo.Game.LocalGame.Elements;
using GamesToGo.Game.Online.Models.RequestModel;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osuTK;

namespace GamesToGo.Game.LocalGame
{
    [Cached]
    public class WorkingGame
    {
        private TextureStore textures;
        private readonly List<GameElement> gameElements = new List<GameElement>();
        public IEnumerable<Board> GameBoards => gameElements.OfType<Board>();
        public IEnumerable<Tile> GameTiles => gameElements.OfType<Tile>();
        public IEnumerable<Card> GameCards => gameElements.OfType<Card>();
        public IEnumerable<Token> GameTokens => gameElements.OfType<Token>();
        public IList<GameElement> GameElements => gameElements;

        public void Parse(Storage store, TextureStore textures, OnlineGame game)
        {
            this.textures = textures;
            parse(System.IO.File.ReadAllLines(store.GetFullPath($"files/{game.Hash}")));
            postParse();
        }

        private bool parse(IReadOnlyList<string> lines)
        {
            bool isParsingObjects = false;

            List<string> objects = new List<string>();

            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.StartsWith('['))
                {
                    isParsingObjects = line.Trim('[', ']') switch
                    {
                        "Info" => false,
                        "Objects" => true,
                        _ => isParsingObjects,
                    };

                    continue;
                }

                if (isParsingObjects)
                {
                    objects.Add(line);
                }
            }

            return parseObjects(objects);
        }

        private bool parseObjects(IList<string>objects)
        {
            List<string> aux = new List<string>();

            List<string> tokens = new List<string>();
            List<string> cards = new List<string>();
            List<string> tiles = new List<string>();
            List<string> boards = new List<string>();

            for (int i = 0; i < objects.Count; i++)
            {
                var line = objects[i];
                aux.Add(line);
                if (string.IsNullOrEmpty(line) || i == objects.Count-1)
                {
                    var info = aux[0].Split('|', 3);
                    switch(Enum.Parse<ElementType>(info[0]))
                    {
                        case ElementType.Token:
                        {
                            foreach(var l in aux)
                            {
                                tokens.Add(l);
                            }
                        }break;
                        case ElementType.Card:
                        {
                            foreach (var l in aux)
                            {
                                cards.Add(l);
                            }
                        }
                        break;
                        case ElementType.Tile:
                        {
                            foreach (var l in aux)
                            {
                                tiles.Add(l);
                            }
                        }
                        break;
                        case ElementType.Board:
                        {
                            foreach (var l in aux)
                            {
                                boards.Add(l);
                            }
                        }
                        break;
                    }
                    aux.Clear();
                }
            }

            parseTokens(tokens);
            parseCards(cards);
            parseTiles(tiles);
            parseBoards(boards);

            return true;
        }

        private bool parseTokens(IList<string> tokens) 
        {
            List<string> tokenLines = new List<string>();
            Token token = null;

            for (int i = 0; i < tokens.Count(); i++)
            {
                var line = tokens[i];                
                if (string.IsNullOrEmpty(line))
                {
                    for(int h = 0; h < tokenLines.Count(); h++)
                    {
                        var tokenLine = tokenLines[h];
                        if (token == null)
                        {
                            var tInfo = tokenLine.Split('|', 3);
                            if (tInfo.Length != 3)
                                return false;
                            token = new Token
                            {
                                ID = int.Parse(tInfo[1]),
                                Name = tInfo[2]
                            };
                        }
                        else
                        {
                            var prop = tokenLine.Split('=');
                            if (prop.Length != 2)
                                return false;

                            switch (prop[0])
                            {
                                case "Desc":
                                {
                                    token.Description = prop[1];
                                }
                                break;
                                case "Images":
                                {
                                    int amm = int.Parse(prop[1]);
                                    for (int j = h + amm; h < j; h++)
                                    {
                                        var parts = tokenLines[h + 1].Split('=');
                                        if (parts.Length != 2)
                                            return false;
                                        if (parts[1] == "null")
                                            continue;
                                        else
                                            token.Images.Add(textures.Get($"files/{parts[1]}"));
                                    }
                                }
                                break;
                                case "Privacy":
                                {
                                    token.Privacy = Enum.Parse<ElementPrivacy>(prop[1]);
                                }
                                break;
                            }
                        }
                    }
                    tokenLines.Clear();
                    gameElements.Add(token);
                    token = null;
                }
                else
                    tokenLines.Add(line);
            }
            return true;
        }

        private bool parseCards(IList<string> cards)
        {
            List<string> cardLines = new List<string>();
            Card card = null;

            for (int i = 0; i < cards.Count(); i++)
            {
                var line = cards[i];                
                if (string.IsNullOrEmpty(line))
                {
                    for(int h = 0; h < cardLines.Count(); h++)
                    {
                        var cardLine = cardLines[h];
                        if (card == null)
                        {
                            var cInfo = cardLine.Split('|', 3);
                            if (cInfo.Length != 3)
                                return false;
                            card = new Card
                            {
                                ID = int.Parse(cInfo[1]),
                                Name = cInfo[2]
                            };
                        }
                        else
                        {
                            var prop = cardLine.Split('=');
                            if (prop.Length != 2)
                                return false;

                            switch (prop[0])
                            {
                                case "Desc":
                                {
                                    card.Description = prop[1];
                                }
                                break;
                                case "Images":
                                {
                                    int amm = int.Parse(prop[1]);
                                    for (int j = h + amm; h < j; h++)
                                    {
                                        var parts = cardLines[h + 1].Split('=');
                                        if (parts.Length != 2)
                                            return false;
                                        if (parts[1] == "null")
                                            continue;
                                        else
                                            card.Images.Add(textures.Get($"files/{parts[1]}"));
                                    }
                                }
                                break;
                                case "Size":
                                {
                                    var xy = prop[1].Split("|");
                                    card.Size = new Vector2(float.Parse(xy[0]), float.Parse(xy[1]));
                                }
                                break;
                                case "Privacy":
                                {
                                    card.Privacy = Enum.Parse<ElementPrivacy>(prop[1]);
                                }
                                break;
                                case "Orient":
                                {
                                    card.Orientation = Enum.Parse<ElementOrientation>(prop[1]);                                   
                                }
                                break;
                            }
                        }
                    }
                    cardLines.Clear();
                    gameElements.Add(card);
                    card = null;
                }
                else
                    cardLines.Add(line);
            }
            return true;
        }

        private bool parseTiles(IList<string> tiles)
        {
            List<string> tileLines = new List<string>();
            Tile tile = null;

            for (int i = 0; i < tiles.Count(); i++)
            {
                var line = tiles[i];                
                if (string.IsNullOrEmpty(line) || i == tiles.Count-1)
                {
                    for(int h = 0; h < tileLines.Count(); h++)
                    {
                        var tileLine = tileLines[h];
                        if (tile == null)
                        {
                            var tInfo = tileLine.Split('|', 3);
                            if (tInfo.Length != 3)
                                return false;
                            tile = new Tile
                            {
                                ID = int.Parse(tInfo[1]),
                                Name = tInfo[2]
                            };
                        }
                        else
                        {
                            var prop = tileLine.Split('=');
                            if (prop.Length != 2)
                                return false;

                            switch (prop[0])
                            {
                                case "Desc":
                                {
                                    tile.Description = prop[1];
                                }
                                break;
                                case "Images":
                                {
                                    int amm = int.Parse(prop[1]);
                                    for (int j = h + amm; h < j; h++)
                                    {
                                        var parts = tileLines[h + 1].Split('=');
                                        if (parts.Length != 2)
                                            return false;
                                        if (parts[1] == "null")
                                            continue;
                                        else
                                            tile.Images.Add(textures.Get($"files/{parts[1]}"));
                                    }
                                }
                                break;
                                case "Size":
                                {
                                    var xy = prop[1].Split("|");
                                    tile.Size = new Vector2(float.Parse(xy[0]), float.Parse(xy[1]));
                                }
                                break;
                                case "Orient":
                                {
                                    tile.Orientation = Enum.Parse<ElementOrientation>(prop[1]);
                                }
                                break;
                                case "Position":
                                {
                                    var xy = prop[1].Split("|");
                                    tile.Position = new Vector2(float.Parse(xy[0]), float.Parse(xy[1]));
                                }break;
                                case "Events":
                                {
                                    int amm = int.Parse(prop[1]);
                                    for (int j = h + amm; h < j; h++)
                                    {
                                        var events = tileLines[h + 1].Split('|');
                                        int actions = int.Parse(events[5]);
                                        if(actions != 0)
                                        {
                                            h += actions;
                                        }
                                    }
                                    h += amm;
                                }
                                break;
                            }
                        }
                    }
                    tileLines.Clear();
                    gameElements.Add(tile);
                    tile = null;
                }
                else
                    tileLines.Add(line);
            }
            return true;
        }

        private bool parseBoards(IList<string> boards)
        {
            List<string> boardLines = new List<string>();
            Board board = null;

            for (int i = 0; i < boards.Count(); i++)
            {
                var line = boards[i];                
                if (string.IsNullOrEmpty(line))
                {
                    for(int h = 0; h < boardLines.Count(); h++)
                    {
                        var boardLine = boardLines[h];
                        if (board == null)
                        {
                            var tInfo = boardLine.Split('|', 3);
                            if (tInfo.Length != 3)
                                return false;
                            board = new Board
                            {
                                ID = int.Parse(tInfo[1]),
                                Name = tInfo[2]
                            };
                        }
                        else
                        {
                            var prop = boardLine.Split('=');
                            if (prop.Length != 2)
                                return false;

                            switch (prop[0])
                            {
                                case "Desc":
                                {
                                    board.Description = prop[1];
                                }
                                break;
                                case "Images":
                                {
                                    int amm = int.Parse(prop[1]);
                                    for (int j = h + amm; h < j; h++)
                                    {
                                        var parts = boardLines[h + 1].Split('=');
                                        if (parts.Length != 2)
                                            return false;
                                        if (parts[1] == "null")
                                            continue;
                                        else
                                            board.Images.Add(textures.Get($"files/{parts[1]}"));
                                    }
                                }
                                break;
                                case "Size":
                                {
                                    var xy = prop[1].Split("|");
                                    board.Size = new Vector2(float.Parse(xy[0]), float.Parse(xy[1]));
                                }
                                break;
                                case "SubElems":
                                {
                                    int amm = int.Parse(prop[1]);
                                    for (int j = h + amm; h < j; h++)
                                    {
                                        board.PendingElements.Enqueue(int.Parse(boardLines[h + 1]));
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    boardLines.Clear();
                    gameElements.Add(board);
                    board = null;
                }
                else
                    boardLines.Add(line);
            }
            return true;
        }

        private bool postParse()
        {
            foreach(var board in GameBoards)
            {
                var elementQueue = board.PendingElements;
                while(elementQueue.Count > 0)
                {
                    int nextElement = elementQueue.Peek();
                    Tile tile = GameTiles.FirstOrDefault(e => e.ID == nextElement);

                    if (tile == null)
                        return false;

                    board.Tiles.Add(tile);
                    elementQueue.Dequeue();
                }
            }

            return true;
        }
    }
}
