using System;

namespace GamesToGo.Editor.Project.Arguments
{
    public class PrivacyTypeArgument : Argument, IHasResult
    {
        public override int ArgumentTypeID => 2;

        public override ArgumentType Type => ArgumentType.Privacy;

        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();

        public override string[] Text { get; } =
        {
            @"Privacidad predeterminada",
        };

        public int? Result { get; set; }
    }
}
