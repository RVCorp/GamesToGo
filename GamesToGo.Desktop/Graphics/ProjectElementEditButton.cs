using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Input.Events;

namespace GamesToGo.Desktop.Graphics
{
    public class ProjectElementEditButton : ElementEditButton
    {
        private IBindable<ProjectElement> currentEditing = new Bindable<ProjectElement>();
        private bool selected => (currentEditing.Value?.ID ?? -1) == Element.ID;
        public ProjectElementEditButton()
        {
        }

        [BackgroundDependencyLoader]
        private void load(ProjectEditor editor)
        {
            Action = () => editor.SelectElement(Element);
            currentEditing.BindTo(editor.CurrentEditingElement);
            currentEditing.BindValueChanged((_) =>
            {
                FadeBorder(selected || IsHovered, golden: selected);
            });
            FadeBorder(selected, true, selected);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            if (!selected)
                base.OnHoverLost(e);
        }
    }
}
