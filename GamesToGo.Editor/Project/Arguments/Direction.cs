using System.ComponentModel;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public enum Direction
    {
        Horizontal,
        Vertical,
        [Description(@"Diagonal")]
        DiagonalTopLeft,
        [Description(@"Diagonal invertido")]
        DiagonalTopRight,
    }
}
