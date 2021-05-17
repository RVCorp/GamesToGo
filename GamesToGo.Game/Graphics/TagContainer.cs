using System;
using GamesToGo.Game.Overlays;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using GamesToGo.Common.Game;
using osu.Framework.Extensions;

namespace GamesToGo.Game.Graphics
{
    public class TagContainer : Button
    {
        private Random random = new Random();
        public string Text;
        public Box ColorBox;
        private SpriteIcon icon;
        public Bindable<bool> IsSelected = new Bindable<bool>();
        public uint Value;        


        public TagContainer(Tag tag)
        {
            this.Text = tag.GetDescription();
            Value = (uint)tag;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Height = 150;
            AutoSizeAxes = Axes.X;
            Action = () => toggleIcon();
            Children = new Drawable[]
            {
                ColorBox = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Colour4(randomNumber(), randomNumber(), randomNumber(), 255)
                },
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.Y,
                    AutoSizeAxes = Axes.X,
                    Direction = FillDirection.Horizontal,
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            RelativeSizeAxes = Axes.Y,
                            AutoSizeAxes = Axes.X,
                            Child = new SpriteText
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Text = Text,
                                Font = new FontUsage(size:60)
                            }
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Y,
                            Width = 150,
                            Padding = new MarginPadding(30),
                            Child = icon = new SpriteIcon
                            {
                                RelativeSizeAxes = Axes.Both,
                                Icon = FontAwesome.Solid.CheckCircle
                            }
                        }
                    }
                }
            };
            icon.Hide();
            byte randomNumber()
            {
                return (byte)(random.NextDouble() * 255);
            }
        }

        private void toggleIcon()
        {
            if (IsSelected.Value == false)
            {
                icon.Show();
                IsSelected.Value = true;
            }
            else
            {
                icon.Hide();
                IsSelected.Value = false;
            }
        }
    }
}
