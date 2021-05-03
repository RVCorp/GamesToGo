using System;
using GamesToGo.Common.Game;
using JetBrains.Annotations;
using osu.Framework.Bindables;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class DirectionTypeArgument : Argument, IHasResult
    {
        public override int ArgumentTypeID => 21;
        public override ArgumentReturnType Type => ArgumentReturnType.Direction;
        public override ArgumentReturnType[] ExpectedArguments => Array.Empty<ArgumentReturnType>();

        public override string[] Text { get; } =
        {
            @"Dirección predeterminada",
        };

        public Bindable<int?> Result { get; } = new Bindable<int?>();

        public bool ResultMapsTo(object result) => result is Direction directionValue && (int)directionValue == Result.Value;
    }
}
