using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class AddCardTypeToFirstFreeTileChosenByPlayer : EventAction
    {
        public override int TypeID => 27;

        public override ArgumentReturnType[] ExpectedArguments => new[]
        {
            ArgumentReturnType.CardType,
            ArgumentReturnType.SingleTile,
        };

        public override string[] Text { get; } =
        {
            @"Añadir carta",
            @"a primer casilla libre de columna de la casilla",
        };
    }
}
