namespace GamesToGo.Desktop.Project.Events
{
    public enum ArgumentType
    {
        Single = 0,
        Multiple = 1,
        Type = 1 << 1,

        Player = 1 << 2,
        Number = 1 << 3,
        Card = 1 << 4,
        Tile = 1 << 5,
        Token = 1 << 6,
        Board = 1 << 7,
        Comparison = 1 << 8,

        SinglePlayer = Single | Player,
        MultiplePlayer = Multiple | Player,

        SingleNumber = Single | Number,
        MultipleNumber = Multiple | Number,

        SingleCard = Single | Card,
        MultipleCard = Multiple | Card,
        CardType = Card | Type,

        SingleTile = Single | Tile,
        MultipleTile = Multiple | Tile,
        TileType = Tile | Type,

        SingleToken = Single | Token,
        MultipleToken = Multiple | Token,
        TokenType = Token | Type,

        SingleBoard = Single | Board,
        MultipleBoard = Multiple | Board,
        BoardType = Board | Type,
    }
}
