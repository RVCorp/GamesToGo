using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace GamesToGo.Desktop.Graphics
{
    public class ObjectContainer:FillFlowContainer
    {
        public ObjectContainer ()
        {
            RelativeSizeAxes = Axes.Both;
            Direction = FillDirection.Full;
        }
    }
}
