using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Desktop.Project.Arguments;

namespace GamesToGo.Desktop.Project.Actions
{
    public class PlayerWinsAction : EventAction
    {
        public override int TypeID => 23;

        public override ArgumentType[] ExpectedArguments { get; } ={
            ArgumentType.SinglePlayer,
            ArgumentType.Comparison
        };

        public override string[] Text { get; } = {
            @"El",
            @"gana"
        };
    }
}
