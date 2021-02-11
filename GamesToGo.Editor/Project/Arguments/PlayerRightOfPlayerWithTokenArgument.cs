using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class PlayerRightOfPlayerWithTokenArgument : Argument
    {
        public override int ArgumentTypeID => 5;

        public override ArgumentType Type => ArgumentType.SinglePlayer;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SinglePlayer,
        };

        public override string[] Text { get; } = {
            @"Jugador a la derecha de",
        };
    }
}
