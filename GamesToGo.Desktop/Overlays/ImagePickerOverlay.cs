using GamesToGo.Desktop.Project;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Overlays
{
    public class ImagePickerOverlay : OverlayContainer
    {
        private WorkingProject project;

        [BackgroundDependencyLoader]
        private void load(WorkingProject project)
        {
            this.project = project;

            Children = new[]
            {
                new GridContainer
                {
                    RowDimensions = new[]
                    {
                        new Dimension(),
                        new Dimension(GridSizeMode.Absolute, 430)
                    },
                    Content = new []
                    {
                        new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = Color4.Gainsboro
                            }
                        },
                        new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = Color4.Azure,
                            }
                        },
                    }
                }
            };
        }

        protected override void PopIn()
        {

        }

        public void QueryImage(IProjectElement element)
        {
            Show();
        }

        protected override void PopOut()
        {

        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            Hide();
            return true;
        }
    }
}
