using GamesToGo.Editor.Project.Arguments;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
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
