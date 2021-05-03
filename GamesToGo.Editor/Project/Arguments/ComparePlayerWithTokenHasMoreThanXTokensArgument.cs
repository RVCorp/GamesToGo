using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class ComparePlayerWithTokenHasMoreThanXTokensArgument : Argument
    {
        public override int ArgumentTypeID => 8;

        public override ArgumentReturnType Type => ArgumentReturnType.Comparison;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.SinglePlayer,
            ArgumentReturnType.SingleNumber,
            ArgumentReturnType.TokenType,
        };

        public override string[] Text { get; } = {
            @"si",
            @"tiene más de",
            @"ficha de tipo",
        };
    }
}
