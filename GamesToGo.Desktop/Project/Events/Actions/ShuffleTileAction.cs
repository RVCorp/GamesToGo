using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
{
    public class ShuffleTileAction : EventAction
    {
        public override int TypeID => 11;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SingleTile
        };

        public override string[] Text { get; } ={
            @"Barajear casilla"
        };
    }
}
