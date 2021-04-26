using GamesToGo.Common.Game;
using GamesToGo.Editor.Project.Arguments;

namespace GamesToGo.Editor.Project.Actions
{
    public class PlayerWinsAction : EventAction
    {
        public override int TypeID => 23;

        public override ArgumentReturnType[] ExpectedArguments { get; } ={
            ArgumentReturnType.SinglePlayer,
            ArgumentReturnType.Comparison,
        };

        public override string[] Text { get; } = {
            @"El",
            @"gana",
        };
    }
}
