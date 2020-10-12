using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
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
