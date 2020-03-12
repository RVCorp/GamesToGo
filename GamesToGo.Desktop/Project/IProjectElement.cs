using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Graphics;

namespace GamesToGo.Desktop.Project
{
    public interface IProjectElement
    {
        int ID { get; set; }
        string Name { get; set; }
        Drawable Miniature { get; set; }
    }
}
