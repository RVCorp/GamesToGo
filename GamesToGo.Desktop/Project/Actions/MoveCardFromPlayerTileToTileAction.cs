using GamesToGo.Desktop.Project.Arguments;

namespace GamesToGo.Desktop.Project.Actions
{
    public class MoveCardFromPlayerTileToTileAction : EventAction
    {
        public override int TypeID => 22;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SingleCard,
            ArgumentType.SingleTile,
            ArgumentType.SingleTile
        };

        public override string[] Text { get; } = {
            @"Poner carta",
            @"desde casilla",
            @"a la casilla"
        };
    }
}
