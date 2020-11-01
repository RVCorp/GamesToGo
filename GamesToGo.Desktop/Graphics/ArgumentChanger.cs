using GamesToGo.Desktop.Project.Arguments;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace GamesToGo.Desktop.Graphics
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
    }
}
