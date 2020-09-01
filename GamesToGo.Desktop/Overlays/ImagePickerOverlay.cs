using GamesToGo.Desktop.Project;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using Image = GamesToGo.Desktop.Project.Image;
using osuTK;
using osu.Framework.Graphics.UserInterface;
using GamesToGo.Desktop.Screens;

namespace GamesToGo.Desktop.Overlays
{
    public class ImagePickerOverlay : OverlayContainer
    {
        private WorkingProject project;
        private IBindable<ProjectElement> editing = new Bindable<ProjectElement>();
        private FillFlowContainer<ItemButton> itemsContainer;
        private string targetElementImage;
        private readonly Colour4 gradientLight = Colour4.Black.Opacity(0);
        private readonly Colour4 gradientDark = Colour4.Black.Opacity(0.8f);

        public const float ITEM_SIZE = 300;

        [BackgroundDependencyLoader]
        private void load(WorkingProject project, ProjectEditor editor)
        {
            RelativeSizeAxes = Axes.Both;
            this.project = project;
            editing.BindTo(editor.CurrentEditingElement);

            Alpha = 0;

            Children = new[]
            {
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    ColumnDimensions = new[]
                    {
                        new Dimension()
                    },
                    RowDimensions = new[]
                    {
                        new Dimension(),
                        new Dimension(GridSizeMode.Absolute, 60),
                        new Dimension(GridSizeMode.Absolute, ITEM_SIZE + 10)
                    },
                    Content = new[]
                    {
                        new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = gradientLight,
                            }
                        },
                        new Drawable[]
                        {
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = ColourInfo.GradientVertical(gradientLight, gradientDark),
                                    },
                                    new SpriteText
                                    {
                                        Font = new FontUsage(size: 50),
                                        Anchor = Anchor.CentreLeft,
                                        Origin = Anchor.CentreLeft,
                                        Text = "Selecciona una imagen",
                                    }
                                }
                            }
                        },
                        new Drawable[]
                        {
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = gradientDark,
                                    },
                                    new BasicScrollContainer(Direction.Horizontal)
                                    {
                                        Anchor = Anchor.BottomCentre,
                                        Origin = Anchor.BottomCentre,
                                        RelativeSizeAxes = Axes.Both,
                                        ClampExtension = 30,
                                        Child = itemsContainer = new FillFlowContainer<ItemButton>
                                        {
                                            Direction = FillDirection.Horizontal,
                                            Spacing = new Vector2(15),
                                            Anchor = Anchor.CentreLeft,
                                            Origin = Anchor.CentreLeft,
                                            RelativeSizeAxes = Axes.Y,
                                            AutoSizeAxes = Axes.X,
                                        }
                                    }
                                }
                            }
                        },
                    }
                }
            };

            project.Images.CollectionChanged += (_, __) => recreateItems();

            recreateItems();
        }

        private void recreateItems()
        {
            itemsContainer.Clear();
            foreach (var image in project.Images)
            {
                itemsContainer.Add(new ItemButton(image)
                {
                    Action = () => setImage(image)
                });
            }

            itemsContainer.Add(new ItemButton(null));
        }

        private void setImage(Image image)
        {
            if (targetElementImage == null)
                project.Image.Value = image;
            else
                editing.Value.Images[targetElementImage].Value = image;
            Hide();
        }

        protected override void PopIn()
        {
            this.FadeIn(200);
        }

        public void QueryImage(string imageName)
        {
            targetElementImage = imageName;
            Show();
        }

        public void QueryProjectImage()
        {
            targetElementImage = null;
            Show();
        }

        protected override void PopOut()
        {
            this.FadeOut(200);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            Hide();
            return true;
        }

        private class ItemButton : Button
        {
            private Image image;
            public ItemButton(Image image)
            {
                this.image = image;
                Size = new Vector2(ITEM_SIZE);
                Masking = true;
                CornerRadius = 10;
                BorderThickness = 2;
                BorderColour = Colour4.White;
                Anchor = Anchor.CentreLeft;
                Origin = Anchor.CentreLeft;
                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Black.Opacity(0),
                };
            }

            [BackgroundDependencyLoader]
            private void load(ImageFinderOverlay imageFinder, WorkingProject project)
            {
                if (image == null)
                {
                    Add(new SpriteIcon
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(60),
                        Icon = FontAwesome.Solid.Plus
                    });
                    Action += () => imageFinder.Show(project);
                }
                else
                {
                    Add(new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        FillMode = FillMode.Fit,
                        Texture = image.Texture,
                    });
                }
            }
        }
    }
}
