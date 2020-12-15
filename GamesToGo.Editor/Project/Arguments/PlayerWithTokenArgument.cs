namespace GamesToGo.Editor.Project.Arguments
{
    public class PlayerWithTokenArgument : Argument
    {
        public override int ArgumentTypeID => 6;

        public override ArgumentType Type => ArgumentType.SinglePlayer;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.TokenType,
        };

        public override string[] Text { get; } = {
            @"Jugador con ficha",
        };
    }
}
