using GamesToGo.Common.Game;
using GamesToGo.Editor.Project.Arguments;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class GiveCardFromPlayerToPlayerAction : EventAction
    {
        public override int TypeID => 17;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.SingleCard,
            ArgumentReturnType.SinglePlayer,
            ArgumentReturnType.SinglePlayer,
        };

        public override string[] Text { get; } = {
            @"Dar carta",
            @"desde jugador",
            @"a jugador",
        };
    }
}
