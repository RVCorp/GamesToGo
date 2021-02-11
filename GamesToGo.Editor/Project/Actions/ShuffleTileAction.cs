using GamesToGo.Editor.Project.Arguments;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
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
