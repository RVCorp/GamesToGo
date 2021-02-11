using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace GamesToGo.Editor.Graphics
{
    public class WhiteBarScrollContainer : BasicScrollContainer
    {
        public WhiteBarScrollContainer(Direction scrollDirection = Direction.Vertical) : base(scrollDirection)
        {
            Scrollbar.Blending = BlendingParameters.Additive;
            Scrollbar.Child.Colour = new Colour4(50, 50, 50, 255);
        }
    }
}
