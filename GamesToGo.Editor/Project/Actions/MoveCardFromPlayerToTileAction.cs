using GamesToGo.Common.Game;
using GamesToGo.Editor.Project.Arguments;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class MoveCardFromPlayerToTileAction : EventAction
    {
        public override int TypeID => 8;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.CardType,
            ArgumentReturnType.SinglePlayer,
            ArgumentReturnType.SingleTile,
        };

        public override string[] Text { get; } = {
            @"Poner carta",
            @"desde jugador",
            @"hacia casilla",
        };
    }
}
