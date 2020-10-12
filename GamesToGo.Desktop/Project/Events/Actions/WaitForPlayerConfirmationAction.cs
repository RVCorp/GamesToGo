using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
{
    public class WaitForPlayerConfirmationAction : EventAction
    {
        public override int TypeID => 21;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SinglePlayer
        };

        public override string[] Text { get; } = {
            @"Esperar confirmación de jugador"
        };
    }
}
