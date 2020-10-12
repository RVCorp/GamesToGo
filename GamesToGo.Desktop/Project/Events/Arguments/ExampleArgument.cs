using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events.Arguments
{
    public class ExampleArgument : Argument
    {
        public override int ArgumentTypeID => 1;

        public override ArgumentType Type => ArgumentType.SinglePlayer;

        public override bool HasResult => true;

        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();

        public override string[] Text => Array.Empty<string>();
    }
}
