using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class CompareDirectionHasXTilesWithCardsArgument : Argument
    {
        public override int ArgumentTypeID => 20;

        public override ArgumentReturnType Type => ArgumentReturnType.Comparison;

        public override ArgumentReturnType[] ExpectedArguments { get; } =
        {
            ArgumentReturnType.SingleNumber,
            ArgumentReturnType.Direction,
            ArgumentReturnType.TileType,
            ArgumentReturnType.CardType,
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
