using GamesToGo.Editor.Project;
using osu.Framework.Allocation;

namespace GamesToGo.Editor.Graphics
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
