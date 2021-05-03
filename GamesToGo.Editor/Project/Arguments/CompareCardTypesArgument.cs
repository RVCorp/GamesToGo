using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class CompareCardTypesArgument : Argument
    {
        public override int ArgumentTypeID => 1;

        public override ArgumentReturnType Type => ArgumentReturnType.Comparison;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.SingleCard,
            ArgumentReturnType.CardType,
        };

        public override string[] Text { get; } = {
            @"si",
            @"es de tipo",
        };
    }
}
