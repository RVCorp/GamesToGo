using System;
using GamesToGo.Editor.Project.Arguments;

namespace GamesToGo.Editor.Project.Actions
{
    public class StopTileEventsAction : EventAction
    {
        public override int TypeID => 19;

        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();

        public override string[] Text { get; } = {
            @"Detener eventos de esta casilla",
        };
    }
}
