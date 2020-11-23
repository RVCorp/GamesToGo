using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK;

namespace GamesToGo.Editor.Graphics
{
    public abstract class ProjectSummaryContainer : Container
    {
        protected const float MARGIN_SIZE = 5;
        private const float main_text_size = 35;
        protected const float SMALL_TEXT_SIZE = 20;
        private Container buttonsContainer;
        private FillFlowContainer smallContainer;
        private Container expandedContainer;
        private Container sizedContainer;

        protected Sprite ProjectImage { get; private set; }
        protected FillFlowContainer<IconButton> ButtonFlowContainer { get; private set; }

        protected SpriteText ProjectName { get; private set; }
        protected SpriteText ProjectDescription { get; private set; }
        protected SpriteText UsernameBox { get; private set; }
        protected Container BottomContainer { get; private set; }

        protected Container ImageContainer { get; private set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            Padding = new MarginPadding { Vertical = 3.5f };
            AutoSizeAxes = Axes.Y;
            RelativeSizeAxes = Axes.X;
            Children = new Drawable[]
            {
                sizedContainer = new Container
                {
                    Masking = true,
                    CornerRadius = MARGIN_SIZE,
                    BorderColour = Colour4.DarkGray,
                    BorderThickness = 3,
                    RelativeSizeAxes = Axes.X,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = new Colour4(55, 55, 55, 255),
                            Alpha = 0.8f,
                        },
                        expandedContainer = new Container
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
                                        smallContainer = new FillFlowContainer
                                        {
                                            Direction = FillDirection.Horizontal,
                                            Spacing = new Vector2(MARGIN_SIZE),
                                            AutoSizeAxes = Axes.Both,
                                            Children = new Drawable[]
                                            {
                                                ImageContainer = new Container
                                                {
                                                    Size = new Vector2(main_text_size + SMALL_TEXT_SIZE + MARGIN_SIZE),
                                                    Masking = true,
                                                    CornerRadius = 20 * (main_text_size + SMALL_TEXT_SIZE + MARGIN_SIZE) / 150,
                                                    Child = ProjectImage = new Sprite
                                                    {
                                                        RelativeSizeAxes = Axes.Both,
                                                        FillMode = FillMode.Fit,
                                                        Anchor = Anchor.Centre,
                                                        Origin = Anchor.Centre,
                                                    },
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
                                                            Font = new FontUsage(size: main_text_size),
                                                        },
                                                        ProjectDescription = new SpriteText
                                                        {
                                                            Font = new FontUsage(size: SMALL_TEXT_SIZE),
                                                        },
                                                    },
                                                },
                                            },
                                        },
                                        UsernameBox = new SpriteText
                                        {
                                            Font = new FontUsage(size: SMALL_TEXT_SIZE),
                                        },
                                        BottomContainer = new Container
                                        {
                                            Height = SMALL_TEXT_SIZE,
                                            AutoSizeAxes = Axes.X,
                                        },
                                    },
                                },
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
                                },
                            },
                        },
                    },
                },
            };
        }
        protected override void LoadComplete()
        {
            base.LoadComplete();
            sizedContainer.Height = smallContainer.Height + MARGIN_SIZE * 2;
        }

        protected override bool OnHover(HoverEvent e)
        {
            sizedContainer.ResizeHeightTo(expandedContainer.Height, 100, Easing.InQuad);
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
            sizedContainer.ResizeHeightTo(smallContainer.Height + MARGIN_SIZE * 2, 100, Easing.InQuad);
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
