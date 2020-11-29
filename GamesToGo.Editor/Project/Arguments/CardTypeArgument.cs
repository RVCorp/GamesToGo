using System;

namespace GamesToGo.Editor.Project.Arguments
{
    public class CardTypeArgument : Argument, IHasResult
    {
        public override int ArgumentTypeID => 10;

        public override ArgumentType Type => ArgumentType.CardType;

        public int? Result { get; set; }

        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();

        public override string[] Text { get; } =
        {
            @"Tipo predeterminado de carta",
        };
    }
}
