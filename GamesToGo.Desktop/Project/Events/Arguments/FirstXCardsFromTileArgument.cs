using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events.Arguments
{
    public class FirstXCardsFromTileArgument : Argument
    {
        public override int ArgumentTypeID => 16;

        public override ArgumentType Type => ArgumentType.MultipleCard;

        public override bool HasResult => false;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SingleNumber,
            ArgumentType.TileType
        };

        public override string[] Text { get; } = {
            @"primeras",
            @"cartas de"
        };
    }
}
