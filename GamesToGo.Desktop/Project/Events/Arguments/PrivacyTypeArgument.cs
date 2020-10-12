using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events.Arguments
{
    public class PrivacyTypeArgument : Argument
    {
        public override int ArgumentTypeID => 2;

        public override ArgumentType Type => ArgumentType.Privacy;

        public override bool HasResult => true;

        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();

        public override string[] Text => Array.Empty<string>();
    }
}
