using GamesToGo.Common.Game;
using GamesToGo.Editor.Project.Arguments;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class DelayTileAction : EventAction
    {
        public override int TypeID => 10;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.SingleTile,
            ArgumentReturnType.Number,
        };

        public override string[] Text { get; } = {
            @"Retrasar casilla",
            @"durante",
            @"segundos",
        };
    }
}
