using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Desktop.Project;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace GamesToGo.Desktop.Graphics
{
    public class ElementListContainer: FillFlowContainer
    {
        public ElementListContainer()
        {
            RelativeSizeAxes = Axes.Both;
            Height = 300;
            Direction = FillDirection.Vertical;
        }
        public void AddElement(IProjectElement element)
        {
            Add(new ElementListButton(element));
        }
    }
}
