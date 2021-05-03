using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class GivePlayerATokenTypeFromPlayerAction : EventAction
    {
        public override int TypeID => 5;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.TokenType,
            ArgumentReturnType.SinglePlayer,
            ArgumentReturnType.SinglePlayer,
        };

        public override string[] Text { get; } = {
            @"Dar ficha",
            @"a jugador",
            @"desde jugador",
        };
    }
}
