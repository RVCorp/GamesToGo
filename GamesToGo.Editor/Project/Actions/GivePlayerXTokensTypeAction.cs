using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class GivePlayerXTokensTypeAction : EventAction
    {
        public override int TypeID => 18;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.SingleNumber,
            ArgumentReturnType.TokenType,
            ArgumentReturnType.SinglePlayer,
        };

        public override string[] Text { get; } = {
            @"Dar",
            @"fichas",
            @"a jugador",
        };
    }
}
