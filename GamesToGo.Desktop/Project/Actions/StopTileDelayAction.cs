using System;
using GamesToGo.Desktop.Project.Arguments;

namespace GamesToGo.Desktop.Project.Actions
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
