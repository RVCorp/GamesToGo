using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;

namespace GamesToGo.Game.Graphics
{
    public class GamesToGoButton : BasicButton
    {
        protected override bool OnMouseDown(MouseDownEvent e)
        {
            return true;
        }

        public new SpriteText SpriteText { get => base.SpriteText; set => base.SpriteText = value; }
    }
}
