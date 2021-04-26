using GamesToGo.Common.Game;
using GamesToGo.Editor.Screens;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;

namespace GamesToGo.Editor.Overlays
{
    public class ProjectFileOverlay : OverlayContainer
    {
        protected override bool StartHidden => true;

        [BackgroundDependencyLoader]
        private void load(ProjectEditor editor)
        {
            RelativeSizeAxes = Axes.Both;

            Child = new Container
            {
                Margin = new MarginPadding(15),
                AutoSizeAxes = Axes.Both,
                Masking = true,
                CornerRadius = 10,
                BorderColour = Colour4.White,
                BorderThickness = 5,
                EdgeEffect = new EdgeEffectParameters
                {
                    Type = EdgeEffectType.Glow,
                    Colour = Colour4.White.Opacity(100),
                    Radius = 10,
                    Hollow = true,
                },
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.Black.Opacity(150),
                    },
                    new FillFlowContainer
                    {
                        Margin = new MarginPadding(15),
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(10),
                        Children = new Drawable[]
                        {
                            new ActionButton(FontAwesome.Solid.Save, @"Guardar proyecto")
                            {
                                Action = () => editor.SaveProject(CommunityStatus.Saved),
                            },
                            new ActionButton(FontAwesome.Solid.Upload, @"Subir proyecto")
                            {
                                Action = editor.UploadProject,
                            },
                        },
                    },
                },
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Child.MoveToY(-Child.Height);
        }

        protected override void PopIn()
        {
            Alpha = 1;
            Child.MoveToY(0, 125, Easing.OutBack);
        }

        protected override void PopOut()
        {
            Child.MoveToY(-Child.Height, 125, Easing.InBack).OnComplete(_ => Alpha = 0);
        }

        protected override bool OnClick(ClickEvent e)
        {
            Hide();

            return true;

        }

        private class ActionButton : Button
        {
            private readonly IconUsage icon;
            private readonly string text;
            private Box backgroundBox;

            public ActionButton(IconUsage icon, string text)
            {
                this.icon = icon;
                this.text = text;
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                AutoSizeAxes = Axes.Both;
                Masking = true;
                CornerRadius = 5;

                Children = new Drawable[]
                {
                    backgroundBox = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.White.Opacity(50),
                        Alpha = 0,
                    },
                    new GridContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        ColumnDimensions = new[]
                        {
                            new Dimension(GridSizeMode.AutoSize),
                            new Dimension(GridSizeMode.Absolute, 10),
                            new Dimension(GridSizeMode.AutoSize),
                        },
                        RowDimensions = new[]
                        {
                            new Dimension(GridSizeMode.AutoSize),
                        },
                        Content = new[]
                        {
                            new Drawable[]
                            {
                                new SpriteIcon
                                {
                                    Size = new Vector2(50),
                                    Icon = icon,
                                },
                                null,
                                new Container
                                {
                                    Anchor = Anchor.CentreLeft,
                                    Origin = Anchor.CentreLeft,
                                    AutoSizeAxes = Axes.Both,
                                    Child = new SpriteText
                                    {
                                        Text = text,
                                        Font = new FontUsage(size: 30),
                                        AllowMultiline = true,
                                        MaxWidth = 400,
                                    },
                                },
                            },
                        },
                    },
                };
            }

            protected override bool OnHover(HoverEvent e)
            {
                backgroundBox.FadeIn(150);
                return base.OnHover(e);
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                backgroundBox.FadeOut(150);
                base.OnHoverLost(e);
            }
        }
    }
}
