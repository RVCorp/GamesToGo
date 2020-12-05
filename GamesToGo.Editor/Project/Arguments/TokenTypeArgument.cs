using System;
using GamesToGo.Editor.Project.Elements;
using osu.Framework.Bindables;

namespace GamesToGo.Editor.Project.Arguments
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

        public Bindable<int?> Result { get; } = new Bindable<int?>();

        public bool ResultMapsTo(object result) => result is Token tokenValue && tokenValue.ID == Result.Value;
    }
}
