using GamesToGo.Editor.Project.Arguments;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class DelayTileAction : EventAction
    {
        public override int TypeID => 10;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SingleTile,
            ArgumentType.Number,
        };

        public override string[] Text { get; } = {
            @"Retrasar casilla",
            @"durante",
            @"segundos",
        };
    }
}
