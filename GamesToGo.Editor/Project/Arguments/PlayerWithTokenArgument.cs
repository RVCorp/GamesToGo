using GamesToGo.Common.Game;

namespace GamesToGo.Editor.Project.Arguments
{
    public class PlayerWithTokenArgument : Argument
    {
        public override int ArgumentTypeID => 6;

        public override ArgumentReturnType Type => ArgumentReturnType.SinglePlayer;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.TokenType,
        };

        public override string[] Text { get; } = {
            @"Jugador con ficha",
        };
    }
}
