using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Project.Elements;
using osu.Framework.Allocation;

namespace GamesToGo.Desktop.Graphics
{
    public class BoardObjectManagerContainer : ObjectManagerContainer<Tile, TileButton>
    {
        [BackgroundDependencyLoader]
        private void load(WorkingProject project)
        {
            BindToList(project.ProjectElements);
        }
    }
}
