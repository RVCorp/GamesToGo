using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace GamesToGo.Editor.Graphics
{
    public abstract class ArgumentSelectionDescriptor : Button
    {
        private readonly Bindable<int?> current = new Bindable<int?>();

        public Bindable<int?> Current
        {
            get => current;
            set => current.BindTo(value);
        }

        protected readonly Container SelectionContainer;
        private Container noSelectionContainer;

        protected ArgumentSelectionDescriptor()
        {
            SelectionContainer = new Container
            {
                AutoSizeAxes = Axes.Both,
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Padding = new MarginPadding(4);
            AutoSizeAxes = Axes.Both;
            Child = new FillFlowContainer
            {
                AutoSizeAxes = Axes.Both,
                Spacing = new Vector2(2),
                Direction = FillDirection.Horizontal,
                Children = new []
                {
                    new Container
                    {
                        AutoSizeAxes = Axes.Both,
                        Children = new Drawable[]
                        {
                            SelectionContainer,
                            noSelectionContainer = new Container
                            {
                                AutoSizeAxes = Axes.Both,
                                Child = new SpriteText
                                {
                                    Anchor = Anchor.CentreLeft,
                                    Origin = Anchor.CentreLeft,
                                    Text = @"Sin selección",
                                    Font = new FontUsage(size: 25),
                                },
                            },
                        },
                    },
                    new Container
                    {
                        Size = new Vector2(25),
                        Child = new SpriteIcon
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                            Size = new Vector2(0.75f),
                            Icon = FontAwesome.Solid.ChevronDown,
                        },
                    },
                },
            };

            Current.BindValueChanged(v =>
            {
                if (v.NewValue != null)
                {
                    SelectionContainer.FadeIn();
                    noSelectionContainer.FadeOut();
                }
                else
                {
                    SelectionContainer.FadeOut();
                    noSelectionContainer.FadeIn();
                }
            }, true);
        }
    }
}
