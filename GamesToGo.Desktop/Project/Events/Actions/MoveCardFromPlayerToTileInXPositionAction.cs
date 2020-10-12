using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
{
    public class MoveCardFromPlayerToTileInXPositionAction : EventAction
    {
        public override int TypeID => 9;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.CardType,
            ArgumentType.SinglePlayer,
            ArgumentType.SingleTile,
            ArgumentType.Number
        };

        public override string[] Text { get; } = {
            @"Poner carta",
            @"desde jugador",
            @"hacia casilla",
            @"en posición"
        };
    }
}
