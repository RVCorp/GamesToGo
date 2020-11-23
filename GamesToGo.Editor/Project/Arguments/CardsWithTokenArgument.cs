namespace GamesToGo.Editor.Project.Arguments
{
    public class CardsWithTokenArgument : Argument
    {
        public override int ArgumentTypeID => 18;

        public override ArgumentType Type => ArgumentType.MultipleCard;

        public override bool HasResult => false;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.TokenType,
        };

        public override string[] Text { get; } = {
            @"cartas con fichas",
        };
    }
}
