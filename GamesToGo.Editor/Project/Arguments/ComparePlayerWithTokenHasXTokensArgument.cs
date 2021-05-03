using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class ComparePlayerWithTokenHasXTokensArgument : Argument
    {
        public override int ArgumentTypeID => 7;

        public override ArgumentReturnType Type => ArgumentReturnType.Comparison;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.SinglePlayer,
            ArgumentReturnType.SingleNumber,
            ArgumentReturnType.TokenType,
        };

        public override string[] Text { get; } = {
            @"si",
            @"tiene exactamente",
            @"ficha de tipo",
        };
    }
}
