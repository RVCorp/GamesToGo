using GamesToGo.Editor.Project.Arguments;

namespace GamesToGo.Editor.Project.Actions
{
    public class PlayerWinsAction : EventAction
    {
        public override int TypeID => 23;

        public override ArgumentType[] ExpectedArguments { get; } ={
            ArgumentType.SinglePlayer,
            ArgumentType.Comparison,
        };

        public override string[] Text { get; } = {
            @"El",
            @"gana",
        };
    }
}
