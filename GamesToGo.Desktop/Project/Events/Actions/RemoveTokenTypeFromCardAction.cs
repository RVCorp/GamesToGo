using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
{
    public class RemoveTokenTypeFromCardAction : EventAction
    {
        public override int TypeID => 15;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.TokenType,
            ArgumentType.MultipleCard
        };

        public override string[] Text { get; } = {
            @"Quitar fichas",
            @"de cartas"
        };
    }
}
