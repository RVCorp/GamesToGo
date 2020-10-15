using System;

namespace GamesToGo.Desktop.Project.Arguments
{
    public class CardTypeArgument : Argument
    {
        public override int ArgumentTypeID => 10;

        public override ArgumentType Type => ArgumentType.CardType;

        public override bool HasResult => true;

        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();

        public override string[] Text { get; } =
        {
            @"Tipo predeterminado de carta",
        };
    }
}
