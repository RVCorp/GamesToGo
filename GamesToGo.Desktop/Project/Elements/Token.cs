using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Graphics;

namespace GamesToGo.Desktop.Project.Elements
{
    public class Token: IProjectElement
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public Drawable TokenImage { get; set; }

        public Drawable Image(bool size)
        {
            return TokenImage;
        }
    }
}
