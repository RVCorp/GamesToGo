using System;
using GamesToGo.Common.Game;
using GamesToGo.Editor.Project.Elements;
using osu.Framework.Bindables;

namespace GamesToGo.Editor.Project.Arguments
{
    public class TokenTypeArgument : Argument, IHasResult
    {
        public override int ArgumentTypeID => 4;

        public override ArgumentReturnType Type => ArgumentReturnType.TokenType;

        public override ArgumentReturnType[] ExpectedArguments => Array.Empty<ArgumentReturnType>();

        public override string[] Text { get; } =
        {
            @"Ficha predeterminada",
        };

        public Bindable<int?> Result { get; } = new Bindable<int?>();

        public bool ResultMapsTo(object result) => result is Token tokenValue && tokenValue.ID == Result.Value;
    }
}
