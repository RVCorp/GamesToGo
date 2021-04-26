using GamesToGo.Common.Game;
using GamesToGo.Editor.Project.Arguments;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class WaitForPlayerConfirmationAction : EventAction
    {
        public override int TypeID => 21;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.SinglePlayer,
        };

        public override string[] Text { get; } = {
            @"Esperar confirmación de jugador",
        };
    }
}
