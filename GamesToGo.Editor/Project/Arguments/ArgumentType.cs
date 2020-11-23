// ReSharper disable UnusedMember.Global

using System;

namespace GamesToGo.Editor.Project.Arguments
{
    [Flags]
    public enum ArgumentType
    {
        Default = 0,

        Single = 1 << 0,
        Multiple = 1 << 1,
        Type = 1 << 2,

        Player = 1 << 3,
        Number = 1 << 4,
        Card = 1 << 5,
        Tile = 1 << 6,
        Token = 1 << 7,
        Board = 1 << 8,
        Comparison = 1 << 9,

        Privacy = 1 << 10,
        Orientation = 1 << 11,

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
