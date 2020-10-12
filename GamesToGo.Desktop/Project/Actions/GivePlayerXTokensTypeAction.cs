using GamesToGo.Desktop.Project.Arguments;

namespace GamesToGo.Desktop.Project.Actions
{
    public class GivePlayerXTokensTypeAction : EventAction
    {
        public override int TypeID => 18;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SingleNumber,
            ArgumentType.TokenType,
            ArgumentType.SinglePlayer
        };

        public override string[] Text { get; } = {
            @"Dar",
            @"fichas",
            @"a jugador"
        };
    }
}
