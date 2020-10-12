using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
{
    public class StopTileDelayAction : EventAction
    {
        public override int TypeID => 20;

        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();

        public override string[] Text { get; } = {
            @"Detener retraso de esta casilla"
        };
    }
}
