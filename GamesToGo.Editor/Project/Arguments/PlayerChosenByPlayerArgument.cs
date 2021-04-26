using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class PlayerChosenByPlayerArgument : Argument
    {
        public override int ArgumentTypeID => 19;

        public override ArgumentReturnType Type => ArgumentReturnType.SinglePlayer;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.SinglePlayer,
        };

        public override string[] Text { get; } = {
            @"jugador seleccionado por",
        };
    }
}
