using GamesToGo.Desktop.Project.Arguments;

namespace GamesToGo.Desktop.Project.Actions
{
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
