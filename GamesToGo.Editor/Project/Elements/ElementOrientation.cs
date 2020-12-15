using System;
using System.ComponentModel;
using GamesToGo.Editor.Graphics;

namespace GamesToGo.Editor.Project.Elements
{
    [Flags]
    public enum ElementOrientation
    {
        Vertical = 0,
        Horizontal = 1,
        [IgnoreItem]
        Flipped = 2,
        [Description("Vertical Volteado")]
        VerticalFlipped = Vertical | Flipped,
        [Description("Horizontal Volteado")]
        HorizontalFlipped = Horizontal | Flipped,
    }
}
