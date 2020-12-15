using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;

namespace GamesToGo.Editor.Graphics
{
    public class GamesToGoDropdown<T> : BasicDropdown<T>
    {
        protected override DropdownHeader CreateHeader() => new GamesToGoDropdownHeader();

        protected override DropdownMenu CreateMenu() => new GamesToGoDropdownMenu();

        private class GamesToGoDropdownHeader : DropdownHeader
        {
            private readonly SpriteText label;

            protected override string Label
            {
                get => label.Text;
                set => label.Text = value;
            }

            public GamesToGoDropdownHeader()
            {
                Depth = -10;
                Foreground.Padding = new MarginPadding(5);
                BackgroundColour = FrameworkColour.Green;
                BackgroundColourHover = FrameworkColour.YellowGreen;
                Children = new[]
                {
                    label = new SpriteText
                    {
                        Font = new FontUsage(size: 25),
                        Height = 25,
                    },
                };
            }
        }

        private class GamesToGoDropdownMenu : DropdownMenu
        {
            protected override Menu CreateSubMenu() => new BasicMenu(Direction.Vertical);

            protected override DrawableDropdownMenuItem CreateDrawableDropdownMenuItem(MenuItem item) => new DrawableGamesToGoDropdownMenuItem(item);

            protected override ScrollContainer<Drawable> CreateScrollContainer(Direction direction) => new BasicScrollContainer(direction);

            private class DrawableGamesToGoDropdownMenuItem : DrawableDropdownMenuItem
            {
                public DrawableGamesToGoDropdownMenuItem(MenuItem item)
                    : base(item)
                {
                    Foreground.Padding = new MarginPadding(5);
                    BackgroundColour = FrameworkColour.BlueGreen;
                    BackgroundColourHover = FrameworkColour.Green;
                    BackgroundColourSelected = FrameworkColour.GreenDark;
                }

                protected override Drawable CreateContent() => new SpriteText
                {
                    Font = new FontUsage(size: 25),
                };
            }
        }
    }

    public class IgnoreItemAttribute : Attribute { }
}
