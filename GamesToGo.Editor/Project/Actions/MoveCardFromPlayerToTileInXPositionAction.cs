using GamesToGo.Editor.Project.Arguments;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class MoveCardFromPlayerToTileInXPositionAction : EventAction
    {
        public override int TypeID => 9;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.CardType,
            ArgumentType.SinglePlayer,
            ArgumentType.SingleTile,
            ArgumentType.SingleNumber,
        };

        public override string[] Text { get; } = {
            @"Poner carta",
            @"desde jugador",
            @"hacia casilla",
            @"en posición",
        };
    }
}
