using System;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;


namespace GamesToGo.Desktop.Graphics
{
    public class ArgumentDropdown : Container
    {
        public Enum[] DropedElement;
        private FillFlowContainer<ArgumentItem> addElement;

        public ArgumentDropdown (Enum[] list)
        {
            DropedElement = list;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AutoSizeAxes = Axes.Y;
            Width = 80;
            Add(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.LightGreen,
            });
            Add(addElement = new FillFlowContainer<ArgumentItem>
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Vertical,
            });
            foreach(var element in DropedElement)
            {
                addElement.Add(new ArgumentItem(element.GetDescription()));
            }
        }
        private class ArgumentItem : Button
        {
            private string text;
            public ArgumentItem(string t)
            {
                text = t;
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                Height = 20;
                AutoSizeAxes = Axes.X;
                Child = new Container
                {
                    Children = new Drawable[]
                    {
                        new SpriteText
                        {
                            RelativeSizeAxes = Axes.Y,
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Text = text,
                        },
                    },
                };
            }
        }
    }
}
