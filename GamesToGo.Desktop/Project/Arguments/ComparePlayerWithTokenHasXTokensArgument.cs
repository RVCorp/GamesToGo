namespace GamesToGo.Desktop.Project.Arguments
{
    public class ComparePlayerWithTokenHasXTokensArgument : Argument
    {
        public override int ArgumentTypeID => 7;

        public override ArgumentType Type => ArgumentType.Comparison;

        public override bool HasResult => false;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SinglePlayer,
            ArgumentType.SingleNumber,
            ArgumentType.TokenType,
        };

        public override string[] Text { get; } = {
            @"si",
            @"tiene exactamente",
            @"ficha de tipo",
        };
    }
}
