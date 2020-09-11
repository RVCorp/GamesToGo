using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Graphics
{
    class ActionDescriptor : Container 
    {
        public ActionDescriptor ()
        {
            RelativeSizeAxes = Axes.X;
            Height = 60;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Purple
                },
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Direction = FillDirection.Horizontal,
                    Children = new Drawable[]
                    {
                        new SpriteText
                        {
                            Text = "Texto kkkkkkkkk"
                        },
                        new ArgumentDescriptor
                        {

                        },
                        new SpriteText
                        {
                            Text = "Texto kkkkkkkkk"
                        },
                        new ArgumentDescriptor
                        {

                        }
                    }
                }
            };
        }
    }
}
