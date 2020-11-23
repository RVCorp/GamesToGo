namespace GamesToGo.Editor.Project.Arguments
{
    public class PlayerRightOfPlayerWithTokenArgument : Argument
    {
        public override int ArgumentTypeID => 5;

        public override ArgumentType Type => ArgumentType.SinglePlayer;

        public override bool HasResult => false;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SinglePlayer,
        };

        public override string[] Text { get; } = {
            @"Jugador a la derecha de",
        };
    }
}
