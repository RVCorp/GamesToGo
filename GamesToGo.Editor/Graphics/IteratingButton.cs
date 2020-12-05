using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;

namespace GamesToGo.Editor.Graphics
{
    public class IteratingButton : GamesToGoButton
    {
        private int iterateAction;
        public List<Action> Actions = new List<Action>();

        [BackgroundDependencyLoader]
        private void load()
        {
            iterateAction = 1;
            Action = () => toggleAction();
        }

        private void toggleAction()
        {         
            Actions[iterateAction]?.Invoke();
            if (iterateAction == 2)
                iterateAction = -1;
            iterateAction++;
        }
    }
}
