using System;
using GamesToGo.Editor.Project.Arguments;

namespace GamesToGo.Editor.Project.Events
{
    public class CardMovedToTileEvent : ProjectEvent
    {
        public override int TypeID => 3;

        public override EventSourceActivator Source => EventSourceActivator.SingleCard;

        public override EventSourceActivator Activator => EventSourceActivator.SingleTile;

        public override string[] Text => new[] { @"Al poner esta carta en una casilla" };

        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();
    }
}
