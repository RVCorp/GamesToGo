using GamesToGo.Editor.Project;
using osu.Framework.Allocation;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Testing;

namespace GamesToGo.Tests
{
    [Cached]
    public class GamesToGoTestBrowser : osu.Framework.Game
    {

        [BackgroundDependencyLoader]
        private void load(Storage store)
        {
            Resources.AddStore(new DllResourceStore(@"GamesToGo.Game.dll"));
            Resources.AddStore(new DllResourceStore(@"GamesToGo.Editor.dll"));
            Textures.AddStore(Host.CreateTextureLoaderStore(new OnlineStore()));
            Textures.AddStore(Host.CreateTextureLoaderStore(new StorageBackedResourceStore(store)));
            Textures.AddExtension("");
        }
        protected override void LoadComplete()
        {
            base.LoadComplete();

            ProjectElement.Textures = Textures;

            Add(new TestBrowser("GamesToGo"));
        }
    }
}
