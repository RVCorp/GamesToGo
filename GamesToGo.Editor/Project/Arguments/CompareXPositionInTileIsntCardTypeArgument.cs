using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class CompareXPositionInTileIsNotCardTypeArgument : Argument
    {
        public override int ArgumentTypeID => 15;

        public override ArgumentReturnType Type => ArgumentReturnType.Comparison;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.SingleNumber,
            ArgumentReturnType.CardType,
        };

        public override string[] Text { get; }= {
            @"si la posición",
            @"en esta casilla no es",
        };
    }
}
