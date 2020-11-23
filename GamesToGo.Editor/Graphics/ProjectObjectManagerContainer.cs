using GamesToGo.Editor.Project;
using GamesToGo.Editor.Screens;
using osu.Framework.Allocation;

namespace GamesToGo.Editor.Graphics
{
    public class ProjectObjectManagerContainer<T> : ObjectManagerContainer<T, ProjectElementEditButton> where T : ProjectElement, new()
    {
        private readonly bool shouldStartEditing;

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
