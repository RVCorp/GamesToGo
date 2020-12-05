using System;
using GamesToGo.Editor.Project.Elements;
using osu.Framework.Bindables;

namespace GamesToGo.Editor.Project.Arguments
{
    public class TileTypeArgument : Argument, IHasResult
    {
        public override int ArgumentTypeID => 12;

        public override ArgumentType Type => ArgumentType.TileType;

        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();

        public override string[] Text { get; } =
        {
            @"Casilla predeterminada",
        };

        public Bindable<int?> Result { get; } = new Bindable<int?>();

        public bool ResultMapsTo(object result) => result is Tile tileValue && tileValue.ID == Result.Value;
    }
}
