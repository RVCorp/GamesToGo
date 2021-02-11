using GamesToGo.Editor.Project.Arguments;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class GivePlayerATokenTypeAction : EventAction
    {
        public override int TypeID => 16;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.TokenType,
            ArgumentType.SinglePlayer,
        };

        public override string[] Text { get; } = {
            @"Dar ficha",
            @"a jugador",
        };
    }
}
