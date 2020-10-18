using GamesToGo.Desktop.Overlays;
using GamesToGo.Desktop.Project;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GamesToGo.Desktop.Graphics
{
    public class ProjectImageChangerButton : ImageChangerButton
    {
        protected override bool ShowOutline => false;

        protected override float ImageCornerRadius => 20;

        [BackgroundDependencyLoader]
        private void load(ImagePickerOverlay imagePicker, WorkingProject project)
        {
            Action = imagePicker.QueryProjectImage;
            Editing.BindTo(project.Image);
        }

        protected override Container CreateHoverContent()
        {
            return new Container
            {
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    new SpriteIcon
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Icon = FontAwesome.Regular.Images,
                        Size = new Vector2(60),
                    },
                },
            };
        }

        protected override Box CreateContentShadow() => new Box
        {
            Colour = new Colour4(93, 107, 110, 200),
            RelativeSizeAxes = Axes.Both,
        };
    }
}
