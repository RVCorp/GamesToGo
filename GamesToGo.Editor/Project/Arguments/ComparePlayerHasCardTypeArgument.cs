using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class ComparePlayerHasCardTypeArgument : Argument
    {
        public override int ArgumentTypeID => 13;

        public override ArgumentType Type => ArgumentType.Comparison;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SinglePlayer,
            ArgumentType.CardType,
        };

        public override string[] Text { get; } = {
            @"si",
            @"tiene cartas de tipo",
        };
    }
}
