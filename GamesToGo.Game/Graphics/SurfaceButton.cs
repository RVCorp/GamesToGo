using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;

namespace GamesToGo.Game.Graphics
{
    public class SurfaceButton : Button
    {
        /// <summary>
        /// Background colour for this button. <see cref="Colour4.Transparent"/> by default.
        /// </summary>
        public ColourInfo BackgroundColour
        {
            get => backgroundBox.Colour;
            set => backgroundBox.Colour = value;
        }

        private readonly Container content;
        private readonly Box backgroundBox;

        protected override Container<Drawable> Content => content;

        public SurfaceButton()
        {
            InternalChildren = new Drawable[]
            {
                backgroundBox = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Transparent,
                },
                content = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
        }
    }
}
