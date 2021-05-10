using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class PlayerStatesOfDefeatAction : EventAction
    {
        public override int TypeID => 24;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.SinglePlayer,
            ArgumentReturnType.Comparison,
        };

        public override string[] Text { get; } = {
            @"El",
            @"pierde",
        };
    }
}
