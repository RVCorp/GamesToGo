using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class GivePlayerXCardsFromTileAction : EventAction
    {
        public override int TypeID => 12;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.Number,
            ArgumentReturnType.SingleTile,
            ArgumentReturnType.SinglePlayer,
        };

        public override string[] Text { get; } = {
            @"Dar",
            @"cartas desde casilla",
            @"a jugador",
        };
    }
}
