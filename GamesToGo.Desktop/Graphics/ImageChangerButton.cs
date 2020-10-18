using System;
using GamesToGo.Desktop.Overlays;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using Image = GamesToGo.Desktop.Project.Image;

namespace GamesToGo.Desktop.Graphics
{
    public abstract class ImageChangerButton : Button
    {
        private Container mainContent;
        private Container hoverContainer;
        protected readonly Bindable<Image> Editing = new Bindable<Image>();
        private Sprite image;

        protected virtual bool ShowOutline => true;
        protected virtual float ImageCornerRadius => 0;

        protected Action<Texture> TextureChanged { get; set; }

        private readonly Bindable<Vector2> size = new Bindable<Vector2>(new Vector2(400));
        private Drawable noContentDrawable;

        protected virtual Drawable CreateMainContent(Container contentContainer) => contentContainer;

        protected virtual Drawable CreateNoSelectionContent() => null;

        protected abstract Container CreateHoverContent();

        protected abstract Box CreateContentShadow();

        protected ImageChangerButton(Bindable<Vector2> size)
        {
            this.size.BindTo(size);
        }

        protected ImageChangerButton()
        {
        }

        [BackgroundDependencyLoader]
        private void load(ImagePickerOverlay imagePicker, ProjectEditor editor, WorkingProject project)
        {
            Container contentContainer = new Container
            {
                RelativeSizeAxes = Axes.Both,
            };

            contentContainer.Add(mainContent = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                FillMode = FillMode.Fit,
                Child = new Container
                {
                    Masking = true,
                    BorderColour = Colour4.White,
                    BorderThickness = ShowOutline ? 3.5f : 0,
                    CornerRadius = ImageCornerRadius,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        CreateContentShadow(),
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Padding = new MarginPadding(ShowOutline ? 2 : 0),
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
            });

            if((noContentDrawable = CreateNoSelectionContent()) != null)
                contentContainer.Add(noContentDrawable);

            Children = new[]
            {
                CreateMainContent(contentContainer),
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
                        CreateHoverContent(),
                    },
                },
            };

            size.BindValueChanged(s =>
            {
                mainContent.FillAspectRatio = s.NewValue.X / s.NewValue.Y;
            }, true);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Editing.BindValueChanged(e => changeImage(e.NewValue), true);
        }

        private void changeImage(Image newImage)
        {
            image.Texture = newImage?.Texture;

            if (image.Texture == null)
            {
                if(noContentDrawable != null)
                    noContentDrawable.Alpha = 1;
                mainContent.Alpha = 0;
            }
            else
            {
                if(noContentDrawable != null)
                    noContentDrawable.Alpha = 0;
                mainContent.Alpha = 1;
            }

            TextureChanged?.Invoke(newImage?.Texture);
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
