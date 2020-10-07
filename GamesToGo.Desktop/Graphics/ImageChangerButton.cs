using GamesToGo.Desktop.Overlays;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using Image = GamesToGo.Desktop.Project.Image;

namespace GamesToGo.Desktop.Graphics
{
    public class ImageChangerButton : Button
    {
        public const float DRAW_SIZE = 400;
        private readonly bool isProject;
        private readonly string imageName;
        private readonly Container content;
        private readonly Container hoverContainer;
        private readonly Bindable<Image> editing = new Bindable<Image>();
        private readonly Sprite image;
        private readonly SpriteText changeImageText;
        private readonly SpriteText noImageText;

        private Vector2 renderSize
        {
            get;
            set;
        }

        private readonly Bindable<Vector2> size = new Bindable<Vector2>();

        private Vector2 targetSize
        {
            set
            {
                float ratio = 1;
                if (value.Y / (DRAW_SIZE - 50) > value.X / DRAW_SIZE)
                {
                    if (value.Y > DRAW_SIZE - 50)
                        ratio = value.Y / (DRAW_SIZE - 50);
                }
                else
                {
                    if (value.X > DRAW_SIZE)
                        ratio = value.X / DRAW_SIZE;
                }

                renderSize = new Vector2(value.X / ratio, value.Y / ratio);
                content.Padding = new MarginPadding { Horizontal = (400 - renderSize.X) / 2, Vertical = (400 - 50 - renderSize.Y) / 2 };
            }
        }

        public ImageChangerButton()
        {
            isProject = true;
            Children = new Drawable[]
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
                                CornerRadius = 20,
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        Colour = new Colour4(93, 107, 110, 200),
                                        RelativeSizeAxes = Axes.Both,
                                    },
                                    new Container
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Padding = new MarginPadding(2),
                                        Child = image = new Sprite
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            FillMode = FillMode.Fit,
                                        },
                                    },
                                },
                            },
                        },
                    },
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
                            Colour = Colour4.Black.Opacity(0.5f),
                        },
                        new SpriteIcon
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Icon = FontAwesome.Regular.Images,
                            Size = new Vector2(60),
                        },
                    },
                },
            };
        }

        public ImageChangerButton(string name, Bindable<Vector2> size)
        {
            isProject = false;
            imageName = name;
            Children = new Drawable[]
            {
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    RowDimensions = new[]
                    {
                        new Dimension(),
                        new Dimension(GridSizeMode.Absolute, 50),
                    },
                    ColumnDimensions = new[]
                    {
                        new Dimension(),
                    },
                    Content = new[]
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
                                            BorderColour = Colour4.White,
                                            BorderThickness = 3.5f,
                                            RelativeSizeAxes = Axes.Both,
                                            Children = new Drawable[]
                                            {
                                                new Box
                                                {
                                                    Colour = Colour4.Black.Opacity(0),
                                                    RelativeSizeAxes = Axes.Both,
                                                },
                                                new Container
                                                {
                                                    RelativeSizeAxes = Axes.Both,
                                                    Padding = new MarginPadding(2),
                                                    Child = image = new Sprite
                                                    {
                                                        Anchor = Anchor.Centre,
                                                        Origin = Anchor.Centre,
                                                        FillMode = FillMode.Fit,
                                                    },
                                                },
                                            },
                                        },
                                    },
                                    noImageText = new SpriteText
                                    {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        Font = new FontUsage(size: 40),
                                        Text = @"No se ha agregado imagen",
                                    },
                                },
                            },
                        },
                        new Drawable[]
                        {
                            new SpriteText
                            {
                                Text = name,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Font = new FontUsage(size: 40),
                            },
                        },
                    },
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
                            Colour = Colour4.Black.Opacity(0.5f),
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
                            Font = new FontUsage(size: 40),
                        },
                    },
                },
            };

            this.size.BindTo(size);
            this.size.BindValueChanged(obj => targetSize = obj.NewValue, true);
        }

        public ImageChangerButton(string name) : this(name, new Bindable <Vector2>(new Vector2(400, 400)))
        {
        }

        [BackgroundDependencyLoader]
        private void load(ImagePickerOverlay imagePicker, ProjectEditor editor, WorkingProject project)
        {
            if(isProject)
            {
                Action = imagePicker.QueryProjectImage;
                editing.BindTo(project.Image);
                editing.BindValueChanged(i => image.Texture = i.NewValue?.Texture, true);
            }
            else
            {
                Action = () => imagePicker.QueryImage(imageName);
                editing.BindTo(editor.CurrentEditingElement.Value.Images[imageName]);
                editing.BindValueChanged(newVal =>
                {
                    if (newVal.NewValue?.Texture != null)
                    {
                        if (newVal.NewValue.Texture.Size.X > renderSize.X - 2 || newVal.NewValue.Texture.Size.Y > renderSize.Y - 2)
                        {
                            image.Size = Vector2.One;
                            image.RelativeSizeAxes = Axes.Both;
                        }
                        else
                        {

                            image.Size = newVal.NewValue.Texture.Size;
                            image.RelativeSizeAxes = Axes.None;
                        }
                    }
                    image.Texture = newVal.NewValue?.Texture;
                    if (image.Texture == null)
                    {
                        changeImageText.Text = @"Añadir Imagen";
                        noImageText.Alpha = 1;
                        content.Alpha = 0;
                    }
                    else
                    {
                        changeImageText.Text = @"Cambiar Imagen";
                        noImageText.Alpha = 0;
                        content.Alpha = 1;
                    }
                }, true);
            }
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
