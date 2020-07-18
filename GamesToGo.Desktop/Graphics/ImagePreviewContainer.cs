using System.Collections.Generic;
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
using Image = GamesToGo.Desktop.Project.Image;
using osuTK;
using osuTK.Graphics;
using GamesToGo.Desktop.Project.Elements;

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
                    Direction = FillDirection.Vertical,
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
            //TODO: Mover contenedor de imagenes

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
            private string imageName;
            private DrawSizePreservingFillContainer content;
            private Container hoverContainer;

            public Vector2 TargetSize
            {
                set => content.TargetDrawSize = value;
            }

            public ImageChangerButton(string name, Vector2 size)
            {
                imageName = name;
                Size = new Vector2(400);
                Children = new Drawable[]
                {
                    content = new DrawSizePreservingFillContainer
                    {
                        TargetDrawSize = size,
                        BorderColour = Color4.White,
                        BorderThickness = 2,
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
                            new SpriteText
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.TopCentre,
                                Text = "Cambiar Imagen",
                                Font = new FontUsage(size: 40)
                            }
                        }
                    }
                };
            }

            public ImageChangerButton(string name) : this(name, new Vector2(400))
            {
            }

            [BackgroundDependencyLoader]
            private void load(ImagePickerOverlay imagePicker)
            {
                Action = () => imagePicker.QueryImage(imageName);
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
