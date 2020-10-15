using System;

namespace GamesToGo.Desktop.Project.Arguments
{
    public class PrivacyTypeArgument : Argument
    {
        public override int ArgumentTypeID => 2;

        public override ArgumentType Type => ArgumentType.Privacy;

        public override bool HasResult => true;

        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();

        public override string[] Text { get; } =
        {
            @"Privacidad predeterminada",
        };
    }
}
