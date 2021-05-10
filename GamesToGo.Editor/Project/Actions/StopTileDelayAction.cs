using System;
using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class StopTileDelayAction : EventAction
    {
        public override int TypeID => 20;

        public override ArgumentReturnType[] ExpectedArguments => Array.Empty<ArgumentReturnType>();

        public override string[] Text { get; } = {
            @"Detener retraso de esta casilla",
        };
    }
}
