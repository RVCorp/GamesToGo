using GamesToGo.Editor.Project.Arguments;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class PlayerStatesOfDefeatAction : EventAction
    {
        public override int TypeID => 24;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SinglePlayer,
            ArgumentType.Comparison,
        };

        public override string[] Text { get; } = {
            @"El",
            @"pierde",
        };
    }
}
