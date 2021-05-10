using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class GivePlayerATokenTypeAction : EventAction
    {
        public override int TypeID => 16;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.TokenType,
            ArgumentReturnType.SinglePlayer,
        };

        public override string[] Text { get; } = {
            @"Dar ficha",
            @"a jugador",
        };
    }
}
