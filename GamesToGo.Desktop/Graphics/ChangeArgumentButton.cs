using GamesToGo.Desktop.Project.Arguments;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;

namespace GamesToGo.Desktop.Graphics
{
    public class ChangeArgumentButton : Button
    {
        private Box hoverBox;
        private readonly Bindable<Argument> model;
        private readonly ArgumentType type;

        [Resolved]
        private ArgumentTypeListing argumentListing { get; set; }

        public ChangeArgumentButton(ArgumentType argumentType, Bindable<Argument> bindable)
        {
            type = argumentType;
            model = bindable;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Padding = new MarginPadding(4) {Left = 0};

            RelativeSizeAxes = Axes.Y;
            AutoSizeAxes = Axes.X;

            Action = changeTo;

            Child = new Container
            {
                RelativeSizeAxes = Axes.Y,
                AutoSizeAxes = Axes.X,
                Masking = true,
                CornerRadius = 4,
                Children = new Drawable[]
                {
                    hoverBox = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.White.Opacity(0.2f),
                        Alpha = 0,
                    },
                    new Container
                    {
                        Padding = new MarginPadding {Horizontal = 5, Vertical = 5},
                        AutoSizeAxes = Axes.X,
                        RelativeSizeAxes = Axes.Y,
                        Child = new SpriteIcon
                        {
                            Icon = FontAwesome.Solid.SyncAlt,
                            Size = new Vector2(20),
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                        },
                    },
                },
            };
        }

        private void changeTo()
        {
            argumentListing.ShowFor(type, model,
                ToSpaceOfOtherDrawable(new Vector2((Width - 4) / 2, DrawHeight), argumentListing));
        }

        protected override bool OnHover(HoverEvent e)
        {
            base.OnHover(e);

            hoverBox.FadeIn(125);

            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            base.OnHoverLost(e);

            hoverBox.FadeOut(125);
        }
    }
}
