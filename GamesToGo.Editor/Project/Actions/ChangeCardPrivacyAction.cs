using GamesToGo.Editor.Project.Arguments;

namespace GamesToGo.Editor.Project.Actions
{
    public class ChangeCardPrivacyAction : EventAction
    {
        public override int TypeID => 2;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SingleCard,
            ArgumentType.Privacy,
        };

        public override string[] Text { get; } = {
            @"Cambiar privacidad de carta",
            @"a",
        };
    }
}
