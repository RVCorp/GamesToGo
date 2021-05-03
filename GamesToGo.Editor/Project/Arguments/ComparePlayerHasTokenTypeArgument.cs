using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class ComparePlayerHasTokenTypeArgument : Argument
    {
        public override int ArgumentTypeID => 14;

        public override ArgumentReturnType Type => ArgumentReturnType.Comparison;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.MultiplePlayer,
            ArgumentReturnType.TokenType,
        };

        public override string[] Text { get; } = {
            @"si algún",
            @"tiene fichas de tipo",
        };
    }
}
