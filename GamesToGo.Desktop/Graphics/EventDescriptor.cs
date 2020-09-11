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
    class EventDescriptor : Container
    {
        public EventDescriptor()
        {
            Height = 40;
            Width = 300;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Red
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

                        }
                    }
                }
            };
        }
    }
}
