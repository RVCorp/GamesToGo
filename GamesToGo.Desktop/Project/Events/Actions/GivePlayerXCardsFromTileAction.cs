using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
{
    public class GivePlayerXCardsFromTileAction : EventAction
    {
        public override int TypeID => 12;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.Number,
            ArgumentType.SingleTile,
            ArgumentType.SinglePlayer
        };

        public override string[] Text { get; } = {
            @"Dar",
            @"cartas desde casilla",
            @"a jugador"
        };
    }
}
