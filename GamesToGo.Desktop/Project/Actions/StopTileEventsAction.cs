using System;
using GamesToGo.Desktop.Project.Arguments;

namespace GamesToGo.Desktop.Project.Actions
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
