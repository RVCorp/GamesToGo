using GamesToGo.Editor.Project.Arguments;

namespace GamesToGo.Editor.Project.Actions
{
    public class ShuffleTileAction : EventAction
    {
        public override int TypeID => 11;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SingleTile,
        };

        public override string[] Text { get; } ={
            @"Barajear casilla",
        };
    }
}
