using System.Linq;
using GamesToGo.Editor.Project;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;

namespace GamesToGo.Editor.Graphics
{
    public class ElementEditButton : Button
    {
        public ProjectElement Element { get; set; }
        private Container borderContainer;
        private SpriteText elementName;

        private Sprite image;

        [BackgroundDependencyLoader]
        private void load()
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
                    BorderColour = Colour4.White,
                    RelativeSizeAxes = Axes.Both,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0.1f,
                    },
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
                    },
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
            FadeBorder(false, true);

            elementName.Text = Element.Name.Value;
            elementName.Current.BindTo(Element.Name);
            Element.Images.Values.First().BindValueChanged(_ => image.Texture = Element.GetImageWithFallback().Texture, true);
        }

        protected void FadeBorder(bool visible, bool instant = false, bool golden = false)
        {
            borderContainer.FadeTo(visible ? 1 : 0, instant ? 0 : 125);
            borderContainer.Colour = golden ? Colour4.Gold : Colour4.White;
        }

        private void fadeBorder(bool visible)
        {
            borderContainer.FadeTo(visible ? 1 : 0, 125);
        }

        protected override bool OnHover(HoverEvent e)
        {
            fadeBorder(true);
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);

            fadeBorder(false);
        }
    }
}
