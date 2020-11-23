using System;
using GamesToGo.Editor.Overlays;
using GamesToGo.Editor.Project;
using GamesToGo.Editor.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;
using Image = GamesToGo.Editor.Project.Image;

namespace GamesToGo.Editor.Graphics
{
    public abstract class ImageChangerButton : Button
    {
        private ContainedImage mainContent;
        private Container hoverContainer;
        protected readonly Bindable<Image> Editing = new Bindable<Image>();

        protected virtual bool ShowOutline => true;
        protected virtual float ImageCornerRadius => 0;

        protected Action<Texture> TextureChanged { get; set; }

        private readonly Bindable<Vector2> size = new Bindable<Vector2>(new Vector2(400));
        private Drawable noContentDrawable;

        protected virtual Drawable CreateMainContent(Container contentContainer) => contentContainer;

        protected virtual Drawable CreateNoSelectionContent() => null;

        protected abstract Container CreateHoverContent();

        protected abstract Box CreateContentShadow();

        protected ImageChangerButton(Bindable<Vector2> size = null)
        {
            if(size != null)
                this.size = size;
        }

        [BackgroundDependencyLoader]
        private void load(ImagePickerOverlay imagePicker, ProjectEditor editor, WorkingProject project)
        {
            RelativeSizeAxes = Axes.Both;
            CornerRadius = ImageCornerRadius;
            Masking = true;

            Container content = new Container
            {
                CornerRadius = ImageCornerRadius,
                Masking = true,
                RelativeSizeAxes = Axes.Both,
                Child = mainContent = new ContainedImage(ShowOutline, ImageCornerRadius)
                {
                    ImageSize = size,
                    Image = Editing,
                    RelativeSizeAxes = Axes.Both,
                },
            };

            mainContent.UnderImageContent.Add(CreateContentShadow());

            if((noContentDrawable = CreateNoSelectionContent()) != null)
                content.Add(noContentDrawable);

            Children = new[]
            {
                CreateMainContent(content),
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
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Editing.BindValueChanged(e => changeImage(e.NewValue), true);
        }

        private void changeImage(Image newImage)
        {
            if (newImage?.Texture == null)
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
