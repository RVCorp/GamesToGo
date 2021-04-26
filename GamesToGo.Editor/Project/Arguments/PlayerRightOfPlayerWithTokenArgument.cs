using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class PlayerRightOfPlayerWithTokenArgument : Argument
    {
        public override int ArgumentTypeID => 5;

        public override ArgumentReturnType Type => ArgumentReturnType.SinglePlayer;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.SinglePlayer,
        };

        public override string[] Text { get; } = {
            @"Jugador a la derecha de",
        };
    }
}
