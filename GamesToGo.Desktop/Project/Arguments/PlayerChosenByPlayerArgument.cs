namespace GamesToGo.Desktop.Project.Arguments
{
    public class PlayerChosenByPlayerArgument : Argument
    {
        public override int ArgumentTypeID => 19;

        public override ArgumentType Type => ArgumentType.SinglePlayer;

        public override bool HasResult => false;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SinglePlayer,
        };

        public override string[] Text { get; } = {
            @"jugador seleccionado por",
        };
    }
}
