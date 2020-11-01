using System;

namespace GamesToGo.Desktop.Project.Arguments
{
    public class NumberArgument : Argument, IHasResult
    {
        public override int ArgumentTypeID => 3;

        public override ArgumentType Type => ArgumentType.SingleNumber;

        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();

        public override string[] Text { get; } =
        {
            @"Numero predeterminado",
        };

        public int? Result { get; set; }
    }
}
