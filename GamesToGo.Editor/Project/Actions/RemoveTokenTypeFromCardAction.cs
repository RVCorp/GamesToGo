using GamesToGo.Editor.Project.Arguments;

namespace GamesToGo.Editor.Project.Actions
{
    public class RemoveTokenTypeFromCardAction : EventAction
    {
        public override int TypeID => 15;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.TokenType,
            ArgumentType.MultipleCard,
        };

        public override string[] Text { get; } = {
            @"Quitar fichas",
            @"de cartas",
        };
    }
}
