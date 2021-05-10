using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class AddCardToTileChosenByPlayer : EventAction
    {
        public override int TypeID => 25;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.CardType,
            ArgumentReturnType.SingleTile,
        };

        public override string[] Text { get; } = {
            @"Añadir nueva carta",
            @"a casilla",
        };
    }
}
