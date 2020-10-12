using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
{
    public class GivePlayerATokenTypeFromPlayerAction : EventAction
    {
        public override int TypeID => 5;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.TokenType,
            ArgumentType.SinglePlayer,
            ArgumentType.SinglePlayer
        };

        public override string[] Text { get; } = {
            @"Dar ficha",
            @"a jugador",
            @"desde jugador"
        };
    }
}
