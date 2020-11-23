using GamesToGo.Editor.Project.Arguments;

namespace GamesToGo.Editor.Project.Actions
{
    public class RemoveTokenTypeFromPlayerAction : EventAction
    {
        public override int TypeID => 6;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.TokenType,
            ArgumentType.SinglePlayer,
        };

        public override string[] Text { get; } = {
            @"Quitar ficha",
            @"a jugador",
        };
    }
}
