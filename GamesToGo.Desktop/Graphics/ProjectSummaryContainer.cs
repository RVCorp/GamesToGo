using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK.Graphics;
using osuTK;
using GamesToGo.Desktop.Project;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Containers;
using SixLabors.ImageSharp;
using System;

namespace GamesToGo.Desktop.Graphics
{
    public class ProjectSummaryContainer : Container
    {
        private const float margin_size = 5;
        private const float main_text_size = 35;
        private const float small_text_size = 20;
        private const float small_height = main_text_size + small_text_size + margin_size * 3;
        private const float expanded_height = main_text_size + small_text_size * 2 + margin_size * 4;
        private readonly Container buttonsContainer;
        private ActionButton deleteButton;
        private ActionButton editButton;

        public ProjectInfo Project { get; private set; }

        public Action<ProjectInfo> EditAction { private get; set; }
        public Action<ProjectInfo> DeleteAction { private get; set; }

        public ProjectSummaryContainer(ProjectInfo project)
        {
            Project = project;
            Masking = true;
            CornerRadius = margin_size;
            BorderColour = Color4.DarkGray;
            BorderThickness = 3;
            RelativeSizeAxes = Axes.X;
            Height = small_height;
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
                    Padding = new MarginPadding(margin_size),
                    Children = new Drawable[]
                    {
                        new FillFlowContainer
                        {
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(margin_size),
                            AutoSizeAxes = Axes.Both,
                            Children = new Drawable[]
                            {
                                new SpriteText
                                {
                                    Font = new FontUsage(size: main_text_size),
                                    Text = project.Name,
                                },
                                new SpriteText
                                {
                                    Font = new FontUsage(size: small_text_size),
                                    Text = $"De StUpIdUsErNaMe27 (Ultima vez editado {project.LastEdited:dd/MM/yyyy HH:mm})",
                                },
                                new FillFlowContainer
                                {
                                    Direction = FillDirection.Horizontal,
                                    Spacing = new Vector2(margin_size),
                                    AutoSizeAxes = Axes.Both,
                                    Children = new Drawable[]
                                    {
                                        new StatText(FontAwesome.Regular.Clone, project.NumberCards),
                                        new StatText(FontAwesome.Solid.Coins, project.NumberTokens),
                                        new StatText(FontAwesome.Solid.ChessBoard, project.NumberBoards),
                                        new StatText(FontAwesome.Regular.Square, project.NumberBoxes)
                                    }
                                }
                            }
                        }
                    },
                },
                buttonsContainer = new Container
                {
                    Padding = new MarginPadding(margin_size),
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    AutoSizeAxes = Axes.Both,
                    Alpha = 0,
                    Children = new Drawable[]
                    {
                        new FillFlowContainer<ActionButton>
                        {
                            AutoSizeAxes = Axes.Both,
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            Spacing = new Vector2(margin_size),
                            Direction = FillDirection.Horizontal,
                            Children = new []
                            {
                                deleteButton = new ActionButton
                                {
                                    Icon = FontAwesome.Solid.TrashAlt,
                                    Action = () => DeleteAction?.Invoke(project),
                                    ButtonColour = Color4.DarkRed,
                                },
                                editButton = new ActionButton
                                {
                                    Icon = FontAwesome.Solid.Edit,
                                    Action = () => EditAction?.Invoke(project),
                                    ButtonColour = FrameworkColour.Green,
                                }
                            }
                        }
                    }
                },
            };
        }

        protected override bool OnHover(HoverEvent e)
        {
            this.ResizeHeightTo(expanded_height, 100, Easing.InQuad);
            buttonsContainer.FadeIn(100, Easing.InQuad)
                .OnComplete(_ =>
                {
                    deleteButton.Enabled.Value = true;
                    editButton.Enabled.Value = true;
                });
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            this.ResizeHeightTo(small_height, 100, Easing.InQuad);
            buttonsContainer.FadeOut(100, Easing.InQuad)
                .OnComplete(_ =>
                {
                    deleteButton.Enabled.Value = false;
                    editButton.Enabled.Value = false;
                });
            base.OnHoverLost(e);
        }

        private class StatText : FillFlowContainer
        {
            public StatText(IconUsage icon, int count)
            {
                Direction = FillDirection.Horizontal;
                Spacing = new Vector2(margin_size);
                AutoSizeAxes = Axes.Both;
                Children = new Drawable[]
                {
                    new SpriteIcon
                    {
                        Size = new Vector2(small_text_size),
                        Icon = icon
                    },
                    new SpriteText
                    {
                        Font = new FontUsage(size: small_text_size),
                        Text = count.ToString(),
                    },
                    new Container
                    {
                        RelativeSizeAxes = Axes.Y,
                        Width = margin_size,
                    },
                };
            }
        }

        private class ActionButton : Button
        {
            private readonly SpriteIcon icon;
            private readonly Box colourBox;
            private static readonly Color4 base_colour = new Color4(100, 100, 100, 255);
            public ActionButton()
            {
                Masking = true;
                CornerRadius = margin_size;
                Anchor = Anchor.CentreRight;
                Origin = Anchor.CentreRight;
                Size = new Vector2(main_text_size * 1.5f, main_text_size);
                Children = new Drawable[]
                {
                    colourBox = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = base_colour,
                    },
                    new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Padding = new MarginPadding(margin_size),
                        Child = icon = new SpriteIcon
                        {
                            RelativeSizeAxes = Axes.Both,
                        }
                    }
                };

                Enabled.ValueChanged += e =>
                {
                    if (IsHovered && e.NewValue)
                        fadeToColour();
                };
            }

            public IconUsage Icon { set => icon.Icon = value; }

            public Color4 ButtonColour { get; set; }

            private void fadeToColour()
            {
                colourBox.FadeColour(ButtonColour, 100);
            }

            protected override bool OnHover(HoverEvent e)
            {
                if (Enabled.Value)
                    fadeToColour();
                return base.OnHover(e);
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                colourBox.FadeColour(base_colour, 100);
                base.OnHoverLost(e);
            }
        }
    }
}
