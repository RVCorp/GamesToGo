﻿using System;

namespace GamesToGo.Desktop.Project.Arguments
{
    public class TokenTypeArgument : Argument, IHasResult
    {
        public override int ArgumentTypeID => 4;

        public override ArgumentType Type => ArgumentType.TokenType;

        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();

        public override string[] Text { get; } =
        {
            @"Ficha predeterminada",
        };

        public int? Result { get; set; }
    }
}
