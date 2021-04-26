using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class PlayerAtXPosition : Argument
    {
        public override int ArgumentTypeID => 23;
        public override ArgumentType Type => ArgumentType.SinglePlayer;

        public override ArgumentType[] ExpectedArguments => new[]
        {
            ArgumentType.SingleNumber,
        };

        public override string[] Text => new[]
        {
            @"jugador en posición",
        };
    }
}
