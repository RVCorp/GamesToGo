namespace GamesToGo.Desktop.Project.Arguments
{
    public class CompareXPositionInTileIsNotCardTypeArgument : Argument
    {
        public override int ArgumentTypeID => 15;

        public override ArgumentType Type => ArgumentType.Comparison;

        public override bool HasResult => false;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SingleNumber,
            ArgumentType.CardType,
        };

        public override string[] Text { get; }= {
            @"si la posición",
            @"en esta casilla no es",
        };
    }
}
