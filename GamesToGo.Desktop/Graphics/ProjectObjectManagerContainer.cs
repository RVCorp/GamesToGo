using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Desktop.Proyect;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Graphics
{
    public class ProjectObjectManagerContainer<T>: Container where T : IProjectElement
    {
        FillFlowContainer allElements;
        public ProjectObjectManagerContainer()
        {
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Red
                },
                allElements = new ObjectFillFlowContainer(),
                new Container
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    RelativeSizeAxes = Axes.X,
                    Height = 50,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.Beige
                        },
                        new BasicButton
                        {
                            RelativeSizeAxes = Axes.Y,
                            Anchor = Anchor.BottomRight,
                            Origin = Anchor.BottomRight,
                            Width = 70,
                            BackgroundColour = Color4.Red,
                            BorderColour = Color4.Black,
                            BorderThickness = 2.5f,
                            Masking = true,
                            Child =  new SpriteIcon
                            {
                                Icon = FontAwesome.Solid.Ad,
                                Colour = Color4.Black,
                                RelativeSizeAxes = Axes.Y,
                                Width = 40,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                            },
                            Action = () => allElements.Add(new Container
                            {
                                Width = 163,
                                Height = 250,
                                Children = new Drawable[]
                                {
                                    new Box //button
                                    {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = Color4.Aquamarine
                                    }
                                }
                            })
                        }
                    }
                }
            };
        }
    }
}
