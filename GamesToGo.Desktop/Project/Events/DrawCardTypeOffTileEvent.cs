using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
{
    public class DrawCardTypeOffTileEvent : ProjectEvent
    {
        public override int TypeID => 4;

        public override EventSourceActivator Source => EventSourceActivator.SingleTile;

        public override EventSourceActivator Activator => EventSourceActivator.SinglePlayer;

        public override string[] Text => new[] { @"Al tomar una carta de tipo" };

        public override ArgumentType[] ExpectedArguments => new[]
        {
            ArgumentType.CardType,
        };
    }
}
