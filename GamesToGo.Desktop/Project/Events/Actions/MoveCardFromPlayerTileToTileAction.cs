using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
{
    public class MoveCardFromPlayerTileToTileAction : EventAction
    {
        public override int TypeID => 22;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SingleCard,
            ArgumentType.SingleTile,
            ArgumentType.SingleTile
        };

        public override string[] Text { get; } = {
            @"Poner carta",
            @"desde casilla",
            @"a la casilla"
        };
    }
}
