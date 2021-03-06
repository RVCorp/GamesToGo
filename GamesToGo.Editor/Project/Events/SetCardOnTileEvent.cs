﻿using System;
using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Events
{
    [UsedImplicitly]
    public class SetCardOnTileEvent : ProjectEvent
    {
        public override int TypeID => 1;

        public override EventSourceActivator Source => EventSourceActivator.SingleTile;

        public override EventSourceActivator Activator => EventSourceActivator.SingleCard;

        public override string[] Text => new[] { @"Al colocar una carta" };

        public override ArgumentReturnType[] ExpectedArguments => Array.Empty<ArgumentReturnType>();
    }
}
