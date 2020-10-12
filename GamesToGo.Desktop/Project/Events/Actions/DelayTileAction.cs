using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
{
    public class DelayTileAction : EventAction
    {
        public override int TypeID => 10;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SingleTile,
            ArgumentType.Number
        };

        public override string[] Text { get; } = {
            @"Retrasar casilla",
            @"durante",
            @"segundos"
        };
    }
}
