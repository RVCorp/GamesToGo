using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;
using osuTK;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Containers;

namespace GamesToGo.Desktop.Graphics
{
    public abstract class ProjectSummaryContainer : Container
    {
        public const float MARGIN_SIZE = 5;
        public const float MAIN_TEXT_SIZE = 35;
        public const float SMALL_TEXT_SIZE = 20;
        public const float SMALL_HEIGHT = MAIN_TEXT_SIZE + SMALL_TEXT_SIZE + MARGIN_SIZE * 3;
        public const float EXPANDED_HEIGHT = MAIN_TEXT_SIZE + SMALL_TEXT_SIZE * 2 + MARGIN_SIZE * 4;
        private Container buttonsContainer;
        protected Sprite ProjectImage { get; }
        protected SpriteText UsernameBox { get; }
        protected FillFlowContainer<IconButton> ButtonFlowContainer { get; }

        protected Container BottomContainer { get; }
        protected SpriteText ProjectName { get; }

        public Container ImageContainer { get; }

        public ProjectSummaryContainer()
        {
            Masking = true;
            CornerRadius = MARGIN_SIZE;
            BorderColour = Color4.DarkGray;
            BorderThickness = 3;
            RelativeSizeAxes = Axes.X;
            Height = SMALL_HEIGHT;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Color4(55, 55, 55, 255),
                    Alpha = 0.8f,
                },
                new Container
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Padding = new MarginPadding(MARGIN_SIZE),
                    Children = new Drawable[]
                    {
                        new FillFlowContainer
                        {
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(MARGIN_SIZE),
                            AutoSizeAxes = Axes.Both,
                            Children = new Drawable[]
                            {
                                new FillFlowContainer
                                {
                                    Direction = FillDirection.Horizontal,
                                    Spacing = new Vector2(MARGIN_SIZE),
                                    AutoSizeAxes = Axes.Both,
                                    Children = new Drawable[]
                                    {
                                        ImageContainer = new Container
                                        {
                                            Size = new Vector2(MAIN_TEXT_SIZE + SMALL_TEXT_SIZE + MARGIN_SIZE),
                                            Masking = true,
                                            CornerRadius = 20 * (MAIN_TEXT_SIZE + SMALL_TEXT_SIZE + MARGIN_SIZE) / 150,
                                            Child = ProjectImage = new Sprite
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                FillMode = FillMode.Fit,
                                                Anchor = Anchor.Centre,
                                                Origin = Anchor.Centre,
                                            }
                                        },
                                        new FillFlowContainer
                                        {
                                            Direction = FillDirection.Vertical,
                                            Spacing = new Vector2(MARGIN_SIZE),
                                            AutoSizeAxes = Axes.Both,
                                            Children = new[]
                                            {
                                                ProjectName = new SpriteText
                                                {
                                                    Font = new FontUsage(size: MAIN_TEXT_SIZE),
                                                },
                                                UsernameBox = new SpriteText
                                                {
                                                    Font = new FontUsage(size: SMALL_TEXT_SIZE),
                                                },
                                            }
                                        }
                                    }
                                },
                                BottomContainer = new Container
                                {
                                    Height = SMALL_TEXT_SIZE,
                                    AutoSizeAxes = Axes.X,
                                }
                            }
                        }
                    },
                },
                buttonsContainer = new Container
                {
                    Padding = new MarginPadding(MARGIN_SIZE),
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    AutoSizeAxes = Axes.Both,
                    Alpha = 0,
                    Children = new Drawable[]
                    {
                        ButtonFlowContainer = new FillFlowContainer<IconButton>
                        {
                            AutoSizeAxes = Axes.Both,
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            Spacing = new Vector2(MARGIN_SIZE),
                            Direction = FillDirection.Horizontal,
                        }
                    }
                },
            };
        }

        protected override bool OnHover(HoverEvent e)
        {
            this.ResizeHeightTo(EXPANDED_HEIGHT, 100, Easing.InQuad);
            buttonsContainer.FadeIn(100, Easing.InQuad)
                .OnComplete(_ =>
                {
                    foreach (var button in ButtonFlowContainer)
                        button.Enabled.Value = true;
                });
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            this.ResizeHeightTo(SMALL_HEIGHT, 100, Easing.InQuad);
            buttonsContainer.FadeOut(100, Easing.InQuad)
                .OnComplete(_ =>
                {
                    foreach (var button in ButtonFlowContainer)
                        button.Enabled.Value = false;
                });
            base.OnHoverLost(e);
        }
    }
}
