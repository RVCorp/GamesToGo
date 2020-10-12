using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
{
    public class ChangeTokenPrivacyAction : EventAction
    {
        public override int TypeID => 3;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SingleToken,
            ArgumentType.Privacy
        };

        public override string[] Text { get; } = {
            @"Cambiar privacidad de ficha",
            @"a"
        };
    }
}
