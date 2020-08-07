using System.Linq;
using GamesToGo.Desktop.Project;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Graphics
{
    public class ElementEditButton : Button
    {
        private ProjectElement element;
        public ProjectElement Element
        {
            get => element;
            set
            {
                if (value == element)
                    return;

                element = value;
                elementName.Text = value.Name.Value;
                elementName.Current.BindTo(value.Name);
                value.Images.Values.First().BindValueChanged((val) => image.Texture = val.NewValue?.Texture ?? value.DefaultImage.Texture, true);
            }
        }
        private readonly Container borderContainer;
        private readonly SpriteText elementName;

        private Sprite image;

        public ElementEditButton()
        {
            Size = new Vector2(165);
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Masking = true;
            Children = new Drawable[]
            {
                borderContainer = new Container
                {
                    Masking = true,
                    CornerRadius = 10,
                    BorderThickness = 2,
                    BorderColour = Color4.White,
                    RelativeSizeAxes = Axes.Both,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0.1f
                    }
                },
                new Container
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Size = new Vector2(120),
                    Position = new Vector2(0, 15),
                    Child = image = new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        FillMode = FillMode.Fit,
                    }
                },
                elementName = new SpriteText
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Position = new Vector2(0, 135),
                    MaxWidth = 150,
                    Font = new FontUsage(size: 20),
                },
            };
        }

        protected void FadeBorder(bool visible, bool instant = false, bool golden = false)
        {
            borderContainer.FadeTo(visible ? 1 : 0, instant ? 0 : 125);
            borderContainer.Colour = golden ? Color4.Gold : Color4.White;
        }

        protected override bool OnHover(HoverEvent e)
        {
            FadeBorder(true);
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);

            FadeBorder(false);
        }
    }
}
