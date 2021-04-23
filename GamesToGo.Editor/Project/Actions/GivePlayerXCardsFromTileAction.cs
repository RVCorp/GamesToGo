using GamesToGo.Editor.Project.Arguments;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class GivePlayerXCardsFromTileAction : EventAction
    {
        public override int TypeID => 12;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SingleNumber,
            ArgumentType.SingleTile,
            ArgumentType.SinglePlayer,
        };

        public override string[] Text { get; } = {
            @"Dar",
            @"cartas desde casilla",
            @"a jugador",
        };
    }
}
