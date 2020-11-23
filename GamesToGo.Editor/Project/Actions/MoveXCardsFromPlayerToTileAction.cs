using GamesToGo.Editor.Project.Arguments;

namespace GamesToGo.Editor.Project.Actions
{
    public class MoveXCardsFromPlayerToTileAction : EventAction
    {
        public override int TypeID => 13;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SingleNumber,
            ArgumentType.SinglePlayer,
            ArgumentType.SingleTile,
        };

        public override string[] Text { get; } = {
            @"Poner",
            @"cartas desde jugador",
            @"en casilla",
        };
    }
}
