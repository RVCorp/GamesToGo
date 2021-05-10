using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class CardSelectedByPlayer : Argument
    {
        public override int ArgumentTypeID => 25;
        public override ArgumentReturnType Type => ArgumentReturnType.CardType;

        public override ArgumentReturnType[] ExpectedArguments => new[]
        {
            ArgumentReturnType.SinglePlayer,
        };

        public override string[] Text { get; } =
        {
            @"Tipo de carta seleccionada por el jugador",
        };
    }
}
