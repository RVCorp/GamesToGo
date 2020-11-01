using System;

namespace GamesToGo.Desktop.Project.Arguments
{
    public class TileTypeArgument : Argument, IHasResult
    {
        public override int ArgumentTypeID => 12;

        public override ArgumentType Type => ArgumentType.TileType;

        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();

        public override string[] Text { get; } =
        {
            @"Casilla predeterminada",
        };

        public int? Result { get; set; }
    }
}
