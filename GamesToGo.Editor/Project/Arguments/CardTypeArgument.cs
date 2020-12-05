using System;
using GamesToGo.Editor.Project.Elements;
using osu.Framework.Bindables;

namespace GamesToGo.Editor.Project.Arguments
{
    public class CardTypeArgument : Argument, IHasResult
    {
        public override int ArgumentTypeID => 10;

        public override ArgumentType Type => ArgumentType.CardType;

        public Bindable<int?> Result { get; } = new Bindable<int?>();

        public bool ResultMapsTo(object result) => result is Card cardValue && cardValue.ID == Result.Value;

        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();

        public override string[] Text { get; } =
        {
            @"Tipo predeterminado de carta",
        };
    }
}
