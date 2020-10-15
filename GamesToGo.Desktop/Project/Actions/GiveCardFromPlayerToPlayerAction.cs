using GamesToGo.Desktop.Project.Arguments;

namespace GamesToGo.Desktop.Project.Actions
{
    public class GiveCardFromPlayerToPlayerAction : EventAction
    {
        public override int TypeID => 17;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SingleCard,
            ArgumentType.SinglePlayer,
            ArgumentType.SinglePlayer,
        };

        public override string[] Text { get; } = {
            @"Dar carta",
            @"desde jugador",
            @"a jugador",
        };
    }
}
