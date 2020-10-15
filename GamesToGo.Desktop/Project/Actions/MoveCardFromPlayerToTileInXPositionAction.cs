using GamesToGo.Desktop.Project.Arguments;

namespace GamesToGo.Desktop.Project.Actions
{
    public class MoveCardFromPlayerToTileInXPositionAction : EventAction
    {
        public override int TypeID => 9;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.CardType,
            ArgumentType.SinglePlayer,
            ArgumentType.SingleTile,
            ArgumentType.Number,
        };

        public override string[] Text { get; } = {
            @"Poner carta",
            @"desde jugador",
            @"hacia casilla",
            @"en posición",
        };
    }
}
