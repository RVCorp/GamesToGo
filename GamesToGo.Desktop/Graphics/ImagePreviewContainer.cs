using GamesToGo.Desktop.Overlays;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using osuTK.Graphics;
using GamesToGo.Desktop.Project.Elements;
using Image = GamesToGo.Desktop.Project.Image;

namespace GamesToGo.Desktop.Graphics
{
    public class ImagePreviewContainer : Container
    {
        private IBindable<ProjectElement> currentEditing = new Bindable<ProjectElement>();
        private FillFlowContainer<ImageChangerButton> images;
        private Container<MovementButton> arrowsContainer;
        private Bindable<int> imagesIndex = new Bindable<int>(0);
        private MovementButton leftMovementButton;
        private MovementButton rightMovementButton;

        [BackgroundDependencyLoader]
        private void load(ProjectEditor editor)
        {
            Masking = true;
            CornerRadius = 15;
            Height = 400;
            Width = 500;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.3f,
                },
                images = new FillFlowContainer<ImageChangerButton>
                {
                    Direction = FillDirection.Horizontal,
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding { Horizontal = 50 },
                    Spacing = new Vector2(100)
                },
                arrowsContainer = new Container<MovementButton>
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0,
                    Children = new[]
                    {
                        leftMovementButton = new MovementButton(true)
                        {
                            Action = () => imagesIndex.Value--
                        },
                        rightMovementButton = new MovementButton(false)
                        {
                            Action = () => imagesIndex.Value++
                        },
                    }
                }
            };
            currentEditing.BindTo(editor.CurrentEditingElement);
            imagesIndex.BindValueChanged(val => move(val.NewValue));
            currentEditing.BindValueChanged(loadElement, true);
        }

        private void move(int value)
        {
            images.MoveToX(value * -500, 250, Easing.OutQuint);
            leftMovementButton.Enabled.Value = value != 0;
            rightMovementButton.Enabled.Value = value != currentEditing.Value.Images.Count - 1;
        }

        private void loadElement(ValueChangedEvent<ProjectElement> e)
        {
            if (e.NewValue != null)
            {
                images.Clear();

                if (e.NewValue is IHasSize sizedElement)
                {
                    foreach (var image in e.NewValue.Images)
                    {
                        images.Add(new ImageChangerButton(image.Key, sizedElement.Size.Value));
                    }
                }
                else
                {
                    foreach (var image in e.NewValue.Images)
                    {
                        images.Add(new ImageChangerButton(image.Key));
                    }
                }
                imagesIndex.Value = 0;
                imagesIndex.TriggerChange();
            }
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
            private readonly Box hoverBox;

            public MovementButton(bool left)
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
                        Colour = Color4.White.Opacity(0.2f),
                        Alpha = 0,
                    },
                    new SpriteIcon
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Icon = left ? FontAwesome.Solid.CaretLeft : FontAwesome.Solid.CaretRight,
                        Size = new Vector2(40),
                    }
                };

                Enabled.BindValueChanged(enabledChanged);
            }

            private void enabledChanged(ValueChangedEvent<bool> obj)
            {
                Colour = obj.NewValue ? Color4.White : new Color4(100, 100, 100, 255);
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

        private class ImageChangerButton : Button
        {
            private const float draw_size = 400;
            private string imageName;
            private Container content;
            private Container hoverContainer;
            private Bindable<Image> editing = new Bindable<Image>();
            private Sprite image;
            private SpriteText changeImageText;
            private SpriteText noImageText;

            public Vector2 RenderSize
            {
                get;
                private set;
            }

            public Vector2 TargetSize
            {
                set
                {
                    float ratio = 1;
                    if (value.Y / (draw_size - 50) > value.X / draw_size)
                    {
                        if (value.Y > draw_size - 50)
                            ratio = value.Y / (draw_size - 50);
                    }
                    else
                    {
                        if (value.X > draw_size)
                            ratio = value.X / draw_size;
                    }

                    RenderSize = new Vector2(value.X / ratio, value.Y / ratio);
                    content.Padding = new MarginPadding { Horizontal = (400 - RenderSize.X) / 2, Vertical = (400 - 50 - RenderSize.Y) / 2 };
                }
            }

            public ImageChangerButton(string name, Vector2 size)
            {
                imageName = name;
                Size = new Vector2(draw_size);
                Children = new Drawable[]
                {
                    new GridContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        RowDimensions = new Dimension[]
                        {
                            new Dimension(),
                            new Dimension(GridSizeMode.Absolute, 50)
                        },
                        ColumnDimensions = new Dimension[]
                        {
                            new Dimension(),
                        },
                        Content = new Drawable[][]
                        {
                            new Drawable[]
                            {
                                new Container
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Children = new Drawable[]
                                    {
                                        content = new Container
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Child = new Container
                                            {
                                                Masking = true,
                                                BorderColour = Color4.White,
                                                BorderThickness = 3.5f,
                                                RelativeSizeAxes = Axes.Both,
                                                Children = new Drawable[]
                                                {
                                                    new Box
                                                    {
                                                        Colour = Color4.Black.Opacity(0),
                                                        RelativeSizeAxes = Axes.Both,
                                                    },
                                                    new Container
                                                    {
                                                        RelativeSizeAxes = Axes.Both,
                                                        Padding = new MarginPadding(2),
                                                        Child =  image = new Sprite
                                                        {
                                                            Anchor = Anchor.Centre,
                                                            Origin = Anchor.Centre,
                                                            FillMode = FillMode.Fit,
                                                        },
                                                    }
                                                }
                                            }

                                        },
                                        noImageText = new SpriteText
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Font = new FontUsage(size: 40),
                                            Text = "No se ha agregado imagen"
                                        }
                                    },
                                }
                            },
                            new Drawable[]
                            {
                                new SpriteText
                                {
                                    Text = name,
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Font = new FontUsage(size: 40)
                                }
                            }
                        }
                    },
                    hoverContainer = new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = Color4.Black.Opacity(0.5f)
                            },
                            new SpriteIcon
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.BottomCentre,
                                Icon = FontAwesome.Regular.Images,
                                Size = new Vector2(60),
                            },
                            changeImageText = new SpriteText
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.TopCentre,
                                Font = new FontUsage(size: 40)
                            }
                        }
                    }
                };

                TargetSize = size;
            }

            public ImageChangerButton(string name) : this(name, new Vector2(200, 400))
            {
            }

            [BackgroundDependencyLoader]
            private void load(ImagePickerOverlay imagePicker, ProjectEditor editor)
            {
                Action = () => imagePicker.QueryImage(imageName);
                editing.BindTo(editor.CurrentEditingElement.Value.Images[imageName]);
                editing.BindValueChanged(newVal =>
                {
                    if (newVal.NewValue?.Texture != null)
                        image.RelativeSizeAxes = newVal.NewValue.Texture.Size.X > RenderSize.X - 2 || newVal.NewValue.Texture.Size.Y > RenderSize.Y - 2 ? Axes.Both : Axes.None;
                    image.Texture = newVal?.NewValue?.Texture;
                    if (image.Texture == null)
                    {
                        changeImageText.Text = "Añadir Imagen";
                        noImageText.Alpha = 1;
                        content.Alpha = 0;
                    }
                    else
                    {
                        changeImageText.Text = "Cambiar Imagen";
                        noImageText.Alpha = 0;
                        content.Alpha = 1;
                    }
                }, true);
            }

            protected override bool OnHover(HoverEvent e)
            {
                hoverContainer.FadeIn(125);
                return base.OnHover(e);
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                hoverContainer.FadeOut(125);
                base.OnHoverLost(e);
            }
        }
    }
}
