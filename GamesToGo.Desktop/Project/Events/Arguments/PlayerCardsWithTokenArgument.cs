using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events.Arguments
{
    public class PlayerCardsWithTokenArgument : Argument
    {
        public override int ArgumentTypeID => 17;

        public override ArgumentType Type => ArgumentType.MultipleCard;

        public override bool HasResult => false;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SinglePlayer,
            ArgumentType.TokenType
        };

        public override string[] Text { get; } = {
            @"cartas de",
            @"con fichas de tipo"
        };
    }
}
