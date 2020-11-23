using GamesToGo.Editor.Project.Arguments;

namespace GamesToGo.Editor.Project.Actions
{
    public class AddCardToTileAction : EventAction
    {
        public override int TypeID => 1;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.CardType,
            ArgumentType.TileType,
        };

        public override string[] Text { get; } = {
            @"Añadir nueva carta",
            @"a casilla",
        };
    }
}
