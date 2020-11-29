using osu.Framework.Allocation;
using osu.Framework.Testing;

namespace GamesToGo.Tests
{
    [Cached]
    public class GamesToGoTestBrowser : osu.Framework.Game
    {
        protected override void LoadComplete()
        {
            base.LoadComplete();

            Add(new TestBrowser("GamesToGo"));
        }
    }
}
