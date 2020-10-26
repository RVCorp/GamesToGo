using GamesToGo.Desktop.Overlays;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Project.Elements;
using osu.Framework.Allocation;

namespace GamesToGo.Desktop.Graphics
{
    public class BoardObjectManagerContainer : ObjectManagerContainer<Tile, ProjectElementEditButton>
    {
        private TileEditorOverlay tileEditorOverlay;

        [BackgroundDependencyLoader]
        private void load(WorkingProject project, TileEditorOverlay teo)
        {
            tileEditorOverlay = teo;
            BindToList(project.ProjectElements);
        }

        protected override void EditTButton(ProjectElementEditButton button)
        {
            button.Action = () => tileEditorOverlay.ShowElement(button.Element);
        }
    }
}
