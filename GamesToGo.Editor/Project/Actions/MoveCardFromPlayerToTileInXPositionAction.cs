using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class MoveCardFromPlayerToTileInXPositionAction : EventAction
    {
        public override int TypeID => 9;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.CardType,
            ArgumentReturnType.SinglePlayer,
            ArgumentReturnType.SingleTile,
            ArgumentReturnType.Number,
        };

        public override string[] Text { get; } = {
            @"Poner carta",
            @"desde jugador",
            @"hacia casilla",
            @"en posición",
        };
    }
}
