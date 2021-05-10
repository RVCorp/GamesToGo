using GamesToGo.Common.Game;

namespace GamesToGo.Editor.Project.Actions
{
    public class AddCardToTileAction : EventAction
    {
        public override int TypeID => 1;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.CardType,
            ArgumentReturnType.TileType,
        };

        public override string[] Text { get; } = {
            @"Añadir nueva carta",
            @"a casilla predeterminada",
        };
    }
}
