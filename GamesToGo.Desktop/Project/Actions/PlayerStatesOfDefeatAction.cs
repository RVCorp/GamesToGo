using GamesToGo.Desktop.Project.Arguments;

namespace GamesToGo.Desktop.Project.Actions
{
    public class PlayerStatesOfDefeatAction : EventAction
    {
        public override int TypeID => 24;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SinglePlayer,
            ArgumentType.Comparison
        };

        public override string[] Text { get; } = {
            @"El",
            @"pierde"
        };
    }
}
