using System;
using System.ComponentModel;

// ReSharper disable UnusedMember.Global

namespace GamesToGo.Game.LocalGame.Elements
{
    [Flags]
    public enum ElementOrientation
    {
        Vertical = 0,
        Horizontal = 1,
        [Description("Vertical Volteado")]
        VerticalFlipped = 2,
        [Description("Horizontal Volteado")]
        HorizontalFlipped = 3,
    }
}
