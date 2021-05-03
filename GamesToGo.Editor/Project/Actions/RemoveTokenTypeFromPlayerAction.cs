using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class RemoveTokenTypeFromPlayerAction : EventAction
    {
        public override int TypeID => 6;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.TokenType,
            ArgumentReturnType.SinglePlayer,
        };

        public override string[] Text { get; } = {
            @"Quitar ficha",
            @"a jugador",
        };
    }
}
