using GamesToGo.Editor.Project.Arguments;

namespace GamesToGo.Editor.Project.Actions
{
    public class MoveCardFromPlayerToTileAction : EventAction
    {
        public override int TypeID => 8;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.CardType,
            ArgumentType.SinglePlayer,
            ArgumentType.SingleTile,
        };

        public override string[] Text { get; } = {
            @"Poner carta",
            @"desde jugador",
            @"hacia casilla",
        };
    }
}
