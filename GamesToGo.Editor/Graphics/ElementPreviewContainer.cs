using System;
using System.Linq;
using GamesToGo.Editor.Project;
using GamesToGo.Editor.Project.Elements;
using GamesToGo.Editor.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osuTK;

namespace GamesToGo.Editor.Graphics
{
    public class ElementPreviewContainer : Container<ContainedImage>
    {
        private readonly IBindable<ProjectElement> currentEditing = new Bindable<ProjectElement>();
        private ContainedImage mainContent;

        [BackgroundDependencyLoader]
        private void load(ProjectEditor editor)
        {
            Child = mainContent = new ContainedImage(true, 0)
            {
                RelativeSizeAxes = Axes.Both,
            };
            currentEditing.BindTo(editor.CurrentEditingElement);
            currentEditing.BindValueChanged(e => RecreatePreview());
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            RecreatePreview();
        }

        public void RecreatePreview()
        {
            var newElem = currentEditing.Value;

            if (newElem == null)
                return;

            switch (newElem.PreviewMode)
            {
                case ElementPreviewMode.None:
                    return;
                case ElementPreviewMode.ParentWithChildren:
                    newElem = newElem.Parent;
                    break;
            }

            if (newElem is IHasSideVisible sided)
            {
                mainContent.Image = sided.DefaultSide.Value switch
                {
                    ElementSideVisible.Front => newElem.Images.Values.Skip(1).First(),
                    ElementSideVisible.Back => newElem.Images.Values.First(),
                    _ => throw new ArgumentException($"Unexpected Side Visible for {newElem.Name}"),
                };
            }
            else
                mainContent.Image = newElem.Images.Values.First();
            mainContent.ImageSize = sizeFor(newElem);
            mainContent.OverImageContent.Clear();

            if (!(newElem is IHasElements elemented)) return;

            foreach (var element in elemented.Elements)
            {
                mainContent.OverImageContent.Add(getContainedImageFor(element).With(contained =>
                {
                    contained.Size = sizeFor(element).Value / mainContent.ExpectedToRealSizeRatio;
                    contained.Position = positionFor(element) / mainContent.ExpectedToRealSizeRatio;
                }));
            }
        }

        private static ContainedImage getContainedImageFor(ProjectElement element)
        {
            Bindable<Vector2> size = sizeFor(element);

            var created = new ContainedImage(true, 0)
            {
                ImageSize = size,
                Image = element.Images.Values.First(),
            };

            if (element is IHasOrientation orientedElement)
            {
                created.Rotation = (int)orientedElement.DefaultOrientation.Value * -90;
            }

            return created;
        }

        private static Vector2 positionFor(ProjectElement element)
        {
            return element is IHasPosition positionedElement ? positionedElement.Position.Value : Vector2.Zero;
        }

        private static Bindable<Vector2> sizeFor(ProjectElement element)
        {
            return element is IHasSize sizedElement ? sizedElement.Size : new Bindable<Vector2>(new Vector2(400));
        }
    }
}
