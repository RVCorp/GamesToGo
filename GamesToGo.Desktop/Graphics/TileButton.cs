using GamesToGo.Desktop.Overlays;
using osu.Framework.Allocation;

namespace GamesToGo.Desktop.Graphics
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
