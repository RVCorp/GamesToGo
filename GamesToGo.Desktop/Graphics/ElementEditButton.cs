using GamesToGo.Desktop.Project;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osu.Framework.Graphics;
using osuTK.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;

namespace GamesToGo.Desktop.Graphics
{
    public class ElementEditButton : Button
    {
        public readonly IProjectElement Element;
        private readonly Container borderContainer;
        public ElementEditButton(IProjectElement element)
        {
            Element = element;
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
                        Alpha = 0,
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
                    },
                    new SpriteText
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        Position = new Vector2(0, 135),
                        Text = element.Name,
                        MaxWidth = 150,
                        Font = new FontUsage(size: 20),
                    },
            };
        }
        protected override bool OnHover(HoverEvent e)
        {
            borderContainer.FadeIn(125);
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);

            borderContainer.FadeOut(125);
        }
    }
}
