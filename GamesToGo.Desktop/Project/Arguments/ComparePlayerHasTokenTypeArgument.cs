namespace GamesToGo.Desktop.Project.Arguments
{
    public class ComparePlayerHasTokenTypeArgument : Argument
    {
        public override int ArgumentTypeID => 14;

        public override ArgumentType Type => ArgumentType.Comparison;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.MultiplePlayer,
            ArgumentType.TokenType,
        };

        public override string[] Text { get; } = {
            @"si algún",
            @"tiene fichas de tipo",
        };
    }
}
