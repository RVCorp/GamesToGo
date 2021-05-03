using System;
using GamesToGo.Common.Game;
using osu.Framework.Bindables;

namespace GamesToGo.Editor.Project.Arguments
{
    public class NumberArgument : Argument, IHasResult
    {
        public override int ArgumentTypeID => 3;

        public override ArgumentReturnType Type => ArgumentReturnType.SingleNumber;

        public override ArgumentReturnType[] ExpectedArguments => Array.Empty<ArgumentReturnType>();

        public override string[] Text { get; } =
        {
            @"Numero predeterminado",
        };

        public Bindable<int?> Result { get; } = new Bindable<int?>(1);

        public bool ResultMapsTo(object result) => result is int number && number == Result.Value;
    }
}
