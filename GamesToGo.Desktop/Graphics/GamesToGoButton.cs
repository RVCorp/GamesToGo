using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;

namespace GamesToGo.Desktop.Graphics
{
    public class GamesToGoButton : BasicButton
    {
        protected override bool OnMouseDown(MouseDownEvent e)
        {
            return true;
        }
        protected override bool OnClick(ClickEvent e)
        {
            return base.OnClick(e);
        }
    }
}
