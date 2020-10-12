using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events.Arguments
{
    public class ComparePlayerWithTokenHasMoreThanXTokensArgument : Argument
    {
        public override int ArgumentTypeID => 8;

        public override ArgumentType Type => ArgumentType.Comparison;

        public override bool HasResult => false;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SinglePlayer,
            ArgumentType.SingleNumber,
            ArgumentType.TokenType
        };

        public override string[] Text { get; } = {
            @"si",
            @"tiene más de",
            @"ficha de tipo"
        };
    }
}
