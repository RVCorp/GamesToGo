using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
{
    public class MoveCardFromPlayerToTileAction : EventAction
    {
        public override int TypeID => 8;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.CardType,
            ArgumentType.SinglePlayer,
            ArgumentType.SingleTile
        };

        public override string[] Text { get; } = {
            @"Poner carta",
            @"desde jugador",
            @"hacia casilla"
        };
    }
}
