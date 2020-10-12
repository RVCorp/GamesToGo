using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
{
    public class GivePlayerATokenTypeAction : EventAction
    {
        public override int TypeID => 16;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.TokenType,
            ArgumentType.SinglePlayer
        };

        public override string[] Text { get; } = {
            @"Dar ficha",
            @"a jugador"
        };
    }
}
