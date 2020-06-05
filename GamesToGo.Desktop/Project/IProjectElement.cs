using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Bindables;
using osu.Framework.Graphics;

namespace GamesToGo.Desktop.Project
{
    public interface IProjectElement
    {
        int ID { get; set; }

        Bindable<string> Name { get; set; }

        Drawable Image(bool size);
    }
}
