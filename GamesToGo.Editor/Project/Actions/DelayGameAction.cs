using GamesToGo.Common.Game;
using GamesToGo.Editor.Project.Arguments;

namespace GamesToGo.Editor.Project.Actions
{
    public class DelayGameAction : EventAction
    {
        public override int TypeID => 4;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.SingleNumber,
        };

        public override string[] Text { get; } = {
            @"Retrasar juego por",
            @"segundos",
        };
    }
}
