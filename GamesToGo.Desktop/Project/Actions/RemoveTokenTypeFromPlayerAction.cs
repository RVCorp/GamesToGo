using GamesToGo.Desktop.Project.Arguments;

namespace GamesToGo.Desktop.Project.Actions
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
