using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class RemovePlayerAction : EventAction
    {
        public override int TypeID => 7;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.SinglePlayer,
        };

        public override string[] Text { get; } = {
            @"Eliminar jugador",
        };
    }
}
