using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class PlayerAtXPosition : Argument
    {
        public override int ArgumentTypeID => 23;
        public override ArgumentReturnType Type => ArgumentReturnType.SinglePlayer;

        public override ArgumentReturnType[] ExpectedArguments => new[]
        {
            ArgumentReturnType.SingleNumber,
        };

        public override string[] Text => new[]
        {
            @"jugador en posición",
        };
    }
}
