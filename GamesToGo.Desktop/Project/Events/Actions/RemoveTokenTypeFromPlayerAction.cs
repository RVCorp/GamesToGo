using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
{
    public class RemoveTokenTypeFromPlayerAction : EventAction
    {
        public override int TypeID => 6;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.TokenType,
            ArgumentType.SinglePlayer
        };

        public override string[] Text { get; } = {
            @"Quitar ficha",
            @"a jugador"
        };
    }
}
