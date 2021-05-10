using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class MoveXCardsFromPlayerToTileAction : EventAction
    {
        public override int TypeID => 13;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.SingleNumber,
            ArgumentReturnType.SinglePlayer,
            ArgumentReturnType.SingleTile,
        };

        public override string[] Text { get; } = {
            @"Poner",
            @"cartas desde jugador",
            @"en casilla",
        };
    }
}
