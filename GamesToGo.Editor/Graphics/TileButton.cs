using GamesToGo.Editor.Overlays;
using osu.Framework.Allocation;

namespace GamesToGo.Editor.Graphics
{
    public class TileButton : ElementEditButton
    {
        [BackgroundDependencyLoader]
        private void load(TileEditorOverlay tileEditorOverlay)
        {
            Action = () => tileEditorOverlay.ShowElement(Element);
        }
    }
}
