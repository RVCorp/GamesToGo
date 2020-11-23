using GamesToGo.Editor.Project;
using GamesToGo.Editor.Project.Elements;
using GamesToGo.Editor.Screens;
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
    public class ElementVisualEditorContainer : Container
    {
        private readonly IBindable<ProjectElement> currentEditing = new Bindable<ProjectElement>();
        private FillFlowContainer<ElementImageChangerButton> images;
        private Container<MovementButton> arrowsContainer;
        private readonly Bindable<int> imagesIndex = new Bindable<int>();
        private MovementButton leftMovementButton;
        private MovementButton rightMovementButton;
        private StateButton imagesButton;
        private StateButton previewButton;
        private GridContainer stateContainer;
        private ElementPreviewContainer preview;

        [BackgroundDependencyLoader]
        private void load(ProjectEditor editor)
        {
            Masking = true;
            CornerRadius = 15;
            Height = 450;
            Width = 500;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Black,
                    Alpha = 0.3f,
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    RowDimensions = new[]
                    {
                        new Dimension(GridSizeMode.AutoSize),
                        new Dimension(),
                    },
                    Content = new[]
                    {
                        new Drawable[]
                        {
                            new GridContainer
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                RowDimensions = new []
                                {
                                    new Dimension(GridSizeMode.AutoSize),
                                },
                                Content = new[]
                                {
                                    new Drawable[]
                                    {
                                        imagesButton = new StateButton
                                        {
                                            Text = @"Imagenes",
                                            Action = () =>
                                            {
                                                stateContainer.MoveToX(0, 100, Easing.Out);
                                                previewButton.Selected = false;
                                            },
                                        },
                                        previewButton = new StateButton
                                        {
                                            Text = @"Vista Previa",
                                            Action = () =>
                                            {
                                                stateContainer.MoveToX(-1, 100, Easing.Out);
                                                imagesButton.Selected = false;
                                            },
                                        },
                                    },
                                },
                            },
                        },
                        new Drawable[]
                        {
                            stateContainer = new GridContainer
                            {
                                RelativePositionAxes = Axes.X,
                                RelativeSizeAxes = Axes.Both,
                                Width = 2,
                                Content = new[]
                                {
                                    new Drawable[]
                                    {
                                        new Container
                                        {
                                            Masking = true,
                                            CornerRadius = 15,
                                            RelativeSizeAxes = Axes.Both,
                                            Children = new Drawable[]
                                            {
                                                new Container
                                                {
                                                    RelativeSizeAxes = Axes.Both,
                                                    Masking = true,
                                                    Child = images = new FillFlowContainer<ElementImageChangerButton>
                                                    {
                                                        Direction = FillDirection.Horizontal,
                                                        RelativeSizeAxes = Axes.Both,
                                                        RelativePositionAxes = Axes.Both,
                                                        Padding = new MarginPadding {Horizontal = 50},
                                                        Spacing = new Vector2(100),
                                                    },
                                                },
                                                arrowsContainer = new Container<MovementButton>
                                                {
                                                    RelativeSizeAxes = Axes.Both,
                                                    Alpha = 0,
                                                    Children = new[]
                                                    {
                                                        leftMovementButton = new MovementButton(true)
                                                        {
                                                            Action = () => imagesIndex.Value--,
                                                        },
                                                        rightMovementButton = new MovementButton(false)
                                                        {
                                                            Action = () => imagesIndex.Value++,
                                                        },
                                                    },
                                                },
                                            },
                                        },
                                        preview = new ElementPreviewContainer
                                        {
                                            Masking = true,
                                            RelativeSizeAxes = Axes.Both,
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
            };
            currentEditing.BindTo(editor.CurrentEditingElement);
            imagesIndex.BindValueChanged(val =>
            {
                images.MoveToX(-val.NewValue, 250, Easing.OutQuint);
                leftMovementButton.Enabled.Value = val.NewValue != 0;
                rightMovementButton.Enabled.Value = val.NewValue != currentEditing.Value.Images.Count - 1;
            });
            currentEditing.BindValueChanged(loadElement, true);
        }

        public void UpdatePreview()
        {
            if (preview?.IsLoaded ?? false)
                preview.RecreatePreview();
        }

        private void loadElement(ValueChangedEvent<ProjectElement> e)
        {
            if (e.NewValue == null)
                return;

            images.Clear();

            foreach (var image in e.NewValue.Images)
            {
                images.Add(new ElementImageChangerButton(image.Key, e.NewValue is IHasSize sizedElement ? sizedElement.Size : null));
            }

            previewButton.Enabled.Value = e.NewValue.PreviewMode != ElementPreviewMode.None;

            imagesButton.Action?.Invoke();

            imagesIndex.Value = 0;
            imagesIndex.TriggerChange();
        }

        protected override bool OnHover(HoverEvent e)
        {
            arrowsContainer.FadeIn(150);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            arrowsContainer.FadeOut(150);
            base.OnHoverLost(e);
        }

        private class MovementButton : Button
        {
            private readonly bool left;
            private Box hoverBox;

            public MovementButton(bool left)
            {
                this.left = left;
            }

            [BackgroundDependencyLoader]
            private void load()
            {
                Anchor = left ? Anchor.CentreLeft : Anchor.CentreRight;
                Origin = left ? Anchor.CentreLeft : Anchor.CentreRight;
                RelativeSizeAxes = Axes.Y;
                Width = 50;
                Children = new Drawable[]
                {
                    hoverBox = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.White.Opacity(0.2f),
                        Alpha = 0,
                    },
                    new SpriteIcon
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Icon = left ? FontAwesome.Solid.CaretLeft : FontAwesome.Solid.CaretRight,
                        Size = new Vector2(40),
                    },
                };

                Enabled.BindValueChanged(enabledChanged);
            }

            private void enabledChanged(ValueChangedEvent<bool> obj)
            {
                Colour = obj.NewValue ? Colour4.White : new Colour4(100, 100, 100, 255);
            }

            protected override bool OnHover(HoverEvent e)
            {
                hoverBox.FadeIn(150);
                return base.OnHover(e);
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                hoverBox.FadeOut(150);
                base.OnHoverLost(e);
            }
        }

        private class StateButton : Button
        {
            private string text;
            private SpriteText spriteText;

            public string Text
            {
                set
                {
                    if (spriteText != null)
                        spriteText.Text = value;

                    text = value;
                }
            }

            private bool selected;

            public bool Selected
            {
                get => selected;
                set
                {
                    glowBox.FadeTo(value ? 1 : 0, 100);

                    selected = value;
                }
            }

            private Box glowBox;
            private Box shadowBox;

            [BackgroundDependencyLoader]
            private void load()
            {
                AutoSizeAxes = Axes.Y;
                RelativeSizeAxes = Axes.X;
                Children = new Drawable[]
                {
                    glowBox = new Box
                    {
                        Height = 5,
                        RelativeSizeAxes = Axes.X,
                        Anchor = Anchor.BottomCentre,
                        Origin = Anchor.BottomCentre,
                    },
                    new Container
                    {
                        Padding = new MarginPadding(10),
                        AutoSizeAxes = Axes.Y,
                        RelativeSizeAxes = Axes.X,
                        Child = spriteText = new SpriteText
                        {
                            Font = new FontUsage(size: 30),
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Text = text,
                        },
                    },
                    shadowBox = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.Black,
                        Alpha = 0,
                    },
                };

                Enabled.BindValueChanged(val =>
                {
                    Selected &= val.NewValue;
                    shadowBox.FadeTo(val.NewValue ? 0 : 0.25f, 100);
                });

                Action += () => { Selected = true; };
            }
        }
    }
}
