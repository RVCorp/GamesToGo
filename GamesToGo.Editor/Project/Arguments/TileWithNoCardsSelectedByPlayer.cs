using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class TileWithNoCardsSelectedByPlayer : Argument
    {
        public override int ArgumentTypeID => 22;
        public override ArgumentType Type => ArgumentType.SingleTile;

        public override ArgumentType[] ExpectedArguments => new[]
        {
            ArgumentType.SinglePlayer,
        };

        public override string[] Text => new[]
        {
            @"casilla sin cartas seleccionada por",
        };
    }
}
