using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;

namespace GamesToGo.Desktop.Graphics
{
    class ArgumentDescriptor: Container
    {
        public ArgumentDescriptor()
        {
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                new BasicTextBox
                {
                    RelativeSizeAxes = Axes.Both
                }
            };
        }
    }
}
