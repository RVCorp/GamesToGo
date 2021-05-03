using System;
using GamesToGo.Common.Game;
using GamesToGo.Editor.Project.Elements;
using osu.Framework.Bindables;

namespace GamesToGo.Editor.Project.Arguments
{
    public class TileTypeArgument : Argument, IHasResult
    {
        public override int ArgumentTypeID => 12;

        public override ArgumentReturnType Type => ArgumentReturnType.TileType;

        public override ArgumentReturnType[] ExpectedArguments => Array.Empty<ArgumentReturnType>();

        public override string[] Text { get; } =
        {
            @"Casilla predeterminada",
        };

        public Bindable<int?> Result { get; } = new Bindable<int?>();

        public bool ResultMapsTo(object result) => result is Tile tileValue && tileValue.ID == Result.Value;
    }
}
