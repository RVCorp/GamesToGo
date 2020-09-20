using GamesToGo.Desktop.Project;
using osu.Framework.Allocation;

namespace GamesToGo.Desktop.Graphics
{
    public class ProjectObjectListingContainer<T> : ObjectListingContainer<T, ProjectElementEditButton> where T : ProjectElement
    {
        [BackgroundDependencyLoader]
        private void load(WorkingProject project)
        {
            BindToList(project.ProjectElements);
        }
    }
}
