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
    }
}
