using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class ComparePlayerHasCardTypeArgument : Argument
    {
        public override int ArgumentTypeID => 13;

        public override ArgumentReturnType Type => ArgumentReturnType.Comparison;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.SinglePlayer,
            ArgumentReturnType.CardType,
        };

        public override string[] Text { get; } = {
            @"si",
            @"tiene cartas de tipo",
        };
    }
}
