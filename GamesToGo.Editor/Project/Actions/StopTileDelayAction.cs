using System;
using GamesToGo.Editor.Project.Arguments;

namespace GamesToGo.Editor.Project.Actions
{
    public class StopTileDelayAction : EventAction
    {
        public override int TypeID => 20;

        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();

        public override string[] Text { get; } = {
            @"Detener retraso de esta casilla",
        };
    }
}
