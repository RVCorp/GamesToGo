using GamesToGo.Desktop.Project.Arguments;

namespace GamesToGo.Desktop.Project.Actions
{
    public class RemovePlayerAction : EventAction
    {
        public override int TypeID => 7;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SinglePlayer
        };

        public override string[] Text { get; } = {
            @"Eliminar jugador"
        };
    }
}
