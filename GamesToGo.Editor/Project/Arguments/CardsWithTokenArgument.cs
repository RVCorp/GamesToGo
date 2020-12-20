using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class CardsWithTokenArgument : Argument
    {
        public override int ArgumentTypeID => 18;

        public override ArgumentType Type => ArgumentType.MultipleCard;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.TokenType,
        };

        public override string[] Text { get; } = {
            @"cartas con fichas",
        };
    }
}
