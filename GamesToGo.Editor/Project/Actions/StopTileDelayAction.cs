using System;
using GamesToGo.Editor.Project.Arguments;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class StopTileDelayAction : EventAction
    {
        public override int TypeID => 20;

        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();

        public override string[] Text { get; } = {
            @"Detener retraso de esta casilla",
        };
    }
}
