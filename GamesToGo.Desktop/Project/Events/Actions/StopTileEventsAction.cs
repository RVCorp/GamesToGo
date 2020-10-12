using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
{
    public class StopTileEventsAction : EventAction
    {
        public override int TypeID => 19;

        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();

        public override string[] Text { get; } = {
            @"Detener eventos de esta casilla"
        };
    }
}
