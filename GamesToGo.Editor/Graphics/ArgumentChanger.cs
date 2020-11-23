using GamesToGo.Editor.Project.Arguments;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;

namespace GamesToGo.Editor.Graphics
{
    public class ArgumentChanger : Container
    {
        private readonly ArgumentType expectedType;
        private readonly Bindable<Argument> model;
        private Container argumentContainer;

        public ArgumentChanger(ArgumentType expectedType, Bindable<Argument> model)
        {
            this.expectedType = expectedType;
            this.model = model;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Padding = new MarginPadding(4);
            AutoSizeAxes = Axes.Both;
            Anchor = Anchor.CentreLeft;
            Origin = Anchor.CentreLeft;

            Child = new Container
            {
                AutoSizeAxes = Axes.Both,
                Masking = true,
                CornerRadius = 4,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.CornflowerBlue,
                    },
                    new FillFlowContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Horizontal,
                        Children = new Drawable[]
                        {
                            argumentContainer = new Container
                            {
                                AutoSizeAxes = Axes.Both,
                                Padding = new MarginPadding(4) { Right = 2 },
                            },
                            new ChangeArgumentButton(expectedType, model),
                        },
                    },
                },
            };

            model.BindValueChanged(changeArgument, true);
        }

        private void changeArgument(ValueChangedEvent<Argument> obj)
        {
            argumentContainer.Child = new ArgumentDescriptor(obj.NewValue);
        }

        private class ChangeArgumentButton : Button
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
                Padding = new MarginPadding(4) { Left = 0 };

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
}
