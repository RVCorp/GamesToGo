using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events.Arguments
{
    public class CompareXPositionInTileIsntCardTypeArgument : Argument
    {
        public override int ArgumentTypeID => 15;

        public override ArgumentType Type => ArgumentType.Comparison;

        public override bool HasResult => false;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SingleNumber,
            ArgumentType.CardType
        };

        public override string[] Text { get; }= {
            @"si la posición",
            @"en esta casilla no es"
        };
    }
}
