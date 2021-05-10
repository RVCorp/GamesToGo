using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class ComparePlayerHasNoCardTypeArgument : Argument
    {
        public override int ArgumentTypeID => 11;

        public override ArgumentReturnType Type => ArgumentReturnType.Comparison;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.SinglePlayer,
            ArgumentReturnType.CardType,
        };

        public override string[] Text { get; } = {
            @"si",
            @"no tiene cartas de tipo",
        };
    }
}
