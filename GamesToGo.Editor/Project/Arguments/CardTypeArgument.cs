using System;
using GamesToGo.Common.Game;
using GamesToGo.Editor.Project.Elements;
using osu.Framework.Bindables;

namespace GamesToGo.Editor.Project.Arguments
{
    public class CardTypeArgument : Argument, IHasResult
    {
        public override int ArgumentTypeID => 10;

        public override ArgumentReturnType Type => ArgumentReturnType.CardType;

        public Bindable<int?> Result { get; } = new Bindable<int?>();

        public bool ResultMapsTo(object result) => result is Card cardValue && cardValue.ID == Result.Value;

        public override ArgumentReturnType[] ExpectedArguments => Array.Empty<ArgumentReturnType>();

        public override string[] Text { get; } =
        {
            @"Tipo predeterminado de carta",
        };
    }
}
