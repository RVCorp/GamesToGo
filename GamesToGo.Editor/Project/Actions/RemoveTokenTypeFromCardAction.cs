using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class RemoveTokenTypeFromCardAction : EventAction
    {
        public override int TypeID => 15;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.TokenType,
            ArgumentReturnType.MultipleCard,
        };

        public override string[] Text { get; } = {
            @"Quitar fichas",
            @"de cartas",
        };
    }
}
