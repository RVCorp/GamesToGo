using System;
using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class StopTileEventsAction : EventAction
    {
        public override int TypeID => 19;

        public override ArgumentReturnType[] ExpectedArguments => Array.Empty<ArgumentReturnType>();

        public override string[] Text { get; } = {
            @"Detener eventos de esta casilla",
        };
    }
}
