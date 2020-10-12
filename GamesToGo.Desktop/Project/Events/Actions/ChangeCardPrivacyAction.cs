using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
{
    public class ChangeCardPrivacyAction : EventAction
    {
        public override int TypeID => 2;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SingleCard,
            ArgumentType.Privacy
        };

        public override string[] Text { get; } = {
            @"Cambiar privacidad de carta",
            @"a"
        };
    }
}
