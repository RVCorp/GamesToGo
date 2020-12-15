using System;
using osu.Framework.Bindables;

namespace GamesToGo.Editor.Project.Arguments
{
    public class NumberArgument : Argument, IHasResult
    {
        public override int ArgumentTypeID => 3;

        public override ArgumentType Type => ArgumentType.SingleNumber;

        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();

        public override string[] Text { get; } =
        {
            @"Numero predeterminado",
        };

        public Bindable<int?> Result { get; } = new Bindable<int?>(1);

        public bool ResultMapsTo(object result) => result is int number && number == Result.Value;
    }
}
