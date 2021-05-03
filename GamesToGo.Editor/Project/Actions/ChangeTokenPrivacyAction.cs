using GamesToGo.Common.Game;

namespace GamesToGo.Editor.Project.Actions
{
    public class ChangeTokenPrivacyAction : EventAction
    {
        public override int TypeID => 3;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.SingleToken,
            ArgumentReturnType.Privacy,
        };

        public override string[] Text { get; } = {
            @"Cambiar privacidad de ficha",
            @"a",
        };
    }
}
