using System;
using GamesToGo.Common.Game;
using GamesToGo.Editor.Project.Arguments;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Events
{
    [UsedImplicitly]
    public class CardMovedToTileEvent : ProjectEvent
    {
        public override int TypeID => 3;

        public override EventSourceActivator Source => EventSourceActivator.SingleCard;

        public override EventSourceActivator Activator => EventSourceActivator.SingleTile;

        public override string[] Text => new[] { @"Al poner esta carta en una casilla" };

        public override ArgumentReturnType[] ExpectedArguments => Array.Empty<ArgumentReturnType>();
    }
}
