using GamesToGo.Editor.Project.Arguments;

namespace GamesToGo.Editor.Project.Actions
{
    public class MoveCardFromTileToTileAction : EventAction
    {
        public override int TypeID => 22;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SingleCard,
            ArgumentType.SingleTile,
            ArgumentType.SingleTile,
        };

        public override string[] Text { get; } = {
            @"Poner carta",
            @"desde casilla",
            @"a la casilla",
        };
    }
}
