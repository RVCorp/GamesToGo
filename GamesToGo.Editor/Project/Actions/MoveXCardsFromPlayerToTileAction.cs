using GamesToGo.Editor.Project.Arguments;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
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
