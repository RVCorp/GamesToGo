﻿using System;
using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Events
{
    [UsedImplicitly]
    public class DrawCardOffTileEvent : ProjectEvent
    {
        public override int TypeID => 5;

        public override EventSourceActivator Source => EventSourceActivator.SingleTile;

        public override EventSourceActivator Activator => EventSourceActivator.SinglePlayer;

        public override string[] Text => new[] { @"Al tomar una carta" };

        public override ArgumentReturnType[] ExpectedArguments => Array.Empty<ArgumentReturnType>();
    }
}
