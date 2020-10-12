using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
{
    public class DelayGameAction : EventAction
    {
        public override int TypeID => 4;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SingleNumber
        };

        public override string[] Text { get; } = {
            @"Retrasar juego por",
            @"segundos"
        };
    }
}
