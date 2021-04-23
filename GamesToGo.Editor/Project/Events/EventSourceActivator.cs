// ReSharper disable UnusedMember.Global

using System;

namespace GamesToGo.Editor.Project.Events
{
    [Flags]
    public enum EventSourceActivator
    {
        Single = 1,
        Multiple = 1 << 1,

        Player = 1 << 2,
        Card = 1 << 4,
        Tile = 1 << 5,
        Token = 1 << 6,
        Board = 1 << 7,

        SinglePlayer = Single | Player,
        MultiplePlayer = Multiple | Player,

        SingleCard = Single | Card,
        MultipleCard = Multiple | Card,

        SingleTile = Single | Tile,
        MultipleTile = Multiple | Tile,

        SingleToken = Single | Token,
        MultipleToken = Multiple | Token,

        SingleBoard = Single | Board,
        MultipleBoard = Multiple | Board,
    }
}
