using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class TileWithNoCardsSelectedByPlayer : Argument
    {
        public override int ArgumentTypeID => 22;
        public override ArgumentReturnType Type => ArgumentReturnType.SingleTile;

        public override ArgumentReturnType[] ExpectedArguments => new[]
        {
            ArgumentReturnType.SinglePlayer,
        };

        public override string[] Text => new[]
        {
            @"casilla sin cartas seleccionada por",
        };
    }
}
