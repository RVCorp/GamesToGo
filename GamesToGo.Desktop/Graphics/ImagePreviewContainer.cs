using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;

namespace GamesToGo.Desktop.Graphics
{
    public class ImagePreviewContainer : Container
    {
        private IBindable<ProjectElement> currentEditing = new Bindable<ProjectElement>();

        [BackgroundDependencyLoader]
        private void load(ProjectEditor editor)
        {

            currentEditing.BindTo(editor.CurrentEditingElement);
            currentEditing.BindValueChanged(loadElement, true);
        }

        private void loadElement(ValueChangedEvent<ProjectElement> e)
        {
            if(e.NewValue != null)
            {

            }
        }
    }
}
