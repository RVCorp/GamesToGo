using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class CompareDirectionHasXTilesWithCardsArgument : Argument
    {
        public override int ArgumentTypeID => 20;

        public override ArgumentType Type => ArgumentType.Comparison;

        public override ArgumentType[] ExpectedArguments { get; } =
        {
            ArgumentType.SingleNumber,
            ArgumentType.Direction,
            ArgumentType.TileType,
            ArgumentType.CardType,
        };

        public override string[] Text { get; } =
        {
            @"si",
            @"casillas en dirección",
            @"desde casilla",
            @"tienen cartas de tipo",
        };
    }
}
