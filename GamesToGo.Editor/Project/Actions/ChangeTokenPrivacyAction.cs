using GamesToGo.Editor.Project.Arguments;

namespace GamesToGo.Editor.Project.Actions
{
    public class ChangeTokenPrivacyAction : EventAction
    {
        public override int TypeID => 3;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SingleToken,
            ArgumentType.Privacy,
        };

        public override string[] Text { get; } = {
            @"Cambiar privacidad de ficha",
            @"a",
        };
    }
}
