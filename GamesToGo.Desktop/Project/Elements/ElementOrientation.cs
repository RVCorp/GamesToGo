using System;

namespace GamesToGo.Desktop.Project.Elements
{
    [Flags]
    public enum ElementOrientation 
    {
        Vertical = 0,
        Horizontal = 1 << 0,
        Flipped = 1 << 1
    }
}
