namespace GamesToGo.Desktop.Project.Arguments
{
    public class ComparePlayerHasCardTypeArgument : Argument
    {
        public override int ArgumentTypeID => 13;

        public override ArgumentType Type => ArgumentType.Comparison;

        public override bool HasResult => false;

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
