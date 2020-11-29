namespace GamesToGo.Editor.Project.Arguments
{
    public class CompareCardTypesArgument : Argument
    {
        public override int ArgumentTypeID => 1;

        public override ArgumentType Type => ArgumentType.Comparison;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SingleCard,
            ArgumentType.CardType,
        };

        public override string[] Text { get; } = {
            @"si",
            @"es de tipo",
        };
    }
}
