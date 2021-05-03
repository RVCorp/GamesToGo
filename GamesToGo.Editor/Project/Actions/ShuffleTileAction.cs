using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class ShuffleTileAction : EventAction
    {
        public override int TypeID => 11;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.SingleTile,
        };

        public override string[] Text { get; } ={
            @"Barajear casilla",
        };
    }
}
