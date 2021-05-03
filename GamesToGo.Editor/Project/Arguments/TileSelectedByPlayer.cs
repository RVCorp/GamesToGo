using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class TileSelectedByPlayer : Argument
    {
        public override int ArgumentTypeID => 24;
        public override ArgumentReturnType Type => ArgumentReturnType.SingleTile;

        public override ArgumentReturnType[] ExpectedArguments => new[]
        {
            ArgumentReturnType.SinglePlayer,
        };

        public override string[] Text { get; } =
        {
            @"Casilla seleccionada por jugador",
        };
    }
}
