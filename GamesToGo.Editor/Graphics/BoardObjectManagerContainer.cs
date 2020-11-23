using GamesToGo.Editor.Overlays;
using GamesToGo.Editor.Project;
using GamesToGo.Editor.Project.Elements;
using osu.Framework.Allocation;

namespace GamesToGo.Editor.Graphics
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
