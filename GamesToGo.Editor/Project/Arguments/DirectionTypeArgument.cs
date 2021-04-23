using System;
using JetBrains.Annotations;
using osu.Framework.Bindables;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class DirectionTypeArgument : Argument, IHasResult
    {
        public override int ArgumentTypeID => 21;
        public override ArgumentType Type => ArgumentType.Direction;
        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();

        public override string[] Text { get; } =
        {
            @"Dirección predeterminada",
        };

        public Bindable<int?> Result { get; } = new Bindable<int?>();

        public bool ResultMapsTo(object result) => result is Direction directionValue && (int)directionValue == Result.Value;
    }
}
