// ReSharper disable UnusedMember.Global
namespace GamesToGo.Desktop.Project.Events
{
    public enum EventSourceActivator
    {
        Single = 0,
        Multiple = 1,

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
