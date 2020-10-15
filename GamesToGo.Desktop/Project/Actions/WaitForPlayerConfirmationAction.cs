using GamesToGo.Desktop.Project.Arguments;

namespace GamesToGo.Desktop.Project.Actions
{
    public class WaitForPlayerConfirmationAction : EventAction
    {
        public override int TypeID => 21;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SinglePlayer,
        };

        public override string[] Text { get; } = {
            @"Esperar confirmación de jugador",
        };
    }
}
