using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Screens;
using osu.Framework.Allocation;

namespace GamesToGo.Desktop.Graphics
{
    public class ProjectObjectManagerContainer<T> : ObjectManagerContainer<T, ProjectElementEditButton> where T : ProjectElement, new()
    {
        private bool shouldStartEditing;

        public ProjectObjectManagerContainer(bool shouldStartEditing = false)
        {
            this.shouldStartEditing = shouldStartEditing;
        }

        [BackgroundDependencyLoader]
        private void load(ProjectEditor editor, WorkingProject project)
        {
            BindToList(project.ProjectElements);
            ButtonAction = () => editor.AddElement(new T(), shouldStartEditing);
        }
    }
}
