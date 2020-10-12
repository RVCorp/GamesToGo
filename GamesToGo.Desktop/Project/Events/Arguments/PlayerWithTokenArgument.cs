using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events.Arguments
{
    public class PlayerWithTokenArgument : Argument
    {
        public override int ArgumentTypeID => 6;

        public override ArgumentType Type => ArgumentType.SinglePlayer;

        public override bool HasResult => false;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.TokenType
        };

        public override string[] Text { get; } = {
            @"Jugador con ficha"
        };
    }
}
