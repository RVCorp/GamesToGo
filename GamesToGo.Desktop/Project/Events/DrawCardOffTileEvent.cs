using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
{
    public class DrawCardOffTileEvent : ProjectEvent
    {
        public override int TypeID => 5;

        public override EventSourceActivator Source => EventSourceActivator.SingleTile;

        public override EventSourceActivator Activator => EventSourceActivator.SinglePlayer;

        public override string[] Text => new[] { @"Al tomar una carta" };

        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();
    }
}
