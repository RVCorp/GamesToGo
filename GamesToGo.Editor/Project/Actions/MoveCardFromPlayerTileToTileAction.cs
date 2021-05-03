using GamesToGo.Common.Game;
using GamesToGo.Editor.Project.Arguments;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class MoveCardFromTileToTileAction : EventAction
    {
        public override int TypeID => 22;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.SingleCard,
            ArgumentReturnType.SingleTile,
            ArgumentReturnType.SingleTile,
        };

        public override string[] Text { get; } = {
            @"Poner carta",
            @"desde casilla",
            @"a la casilla",
        };
    }
}
