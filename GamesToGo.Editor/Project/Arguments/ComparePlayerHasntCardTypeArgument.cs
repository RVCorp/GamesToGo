using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class ComparePlayerHasNoCardTypeArgument : Argument
    {
        public override int ArgumentTypeID => 11;

        public override ArgumentType Type => ArgumentType.Comparison;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SinglePlayer,
            ArgumentType.CardType,
        };

        public override string[] Text { get; } = {
            @"si",
            @"no tiene cartas de tipo",
        };
    }
}
