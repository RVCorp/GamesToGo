using System;
using System.Collections.Generic;

namespace GamesToGo.Desktop.Project.Events
{
    public class SetCardOnTileEvent : Event
    {
        public override int TypeID => 1;

        public override EventSourceActivator Source => EventSourceActivator.SingleBoard;

        public override EventSourceActivator Activator => EventSourceActivator.SingleCard;

        public override IEnumerable<string> Text => new[] { "Al colocar una carta" };

        public override IEnumerable<ArgumentType> ExpectedArguments => Array.Empty<ArgumentType>();
    }
}
