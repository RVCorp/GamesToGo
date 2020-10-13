using System;
using GamesToGo.Desktop.Project.Arguments;

namespace GamesToGo.Desktop.Project.Events
{
    public class SetCardOnTileEvent : ProjectEvent
    {
        public override int TypeID => 1;

        public override EventSourceActivator Source => EventSourceActivator.SingleTile;

        public override EventSourceActivator Activator => EventSourceActivator.SingleCard;

        public override string[] Text => new[] { @"Al colocar una carta" };

        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();
    }
}
