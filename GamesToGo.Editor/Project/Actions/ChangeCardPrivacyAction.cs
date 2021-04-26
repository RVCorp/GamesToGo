using GamesToGo.Common.Game;
using GamesToGo.Editor.Project.Arguments;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class ChangeCardPrivacyAction : EventAction
    {
        public override int TypeID => 2;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.SingleCard,
            ArgumentReturnType.Privacy,
        };

        public override string[] Text { get; } = {
            @"Cambiar privacidad de carta",
            @"a",
        };
    }
}
