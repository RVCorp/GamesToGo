using System;
using System.Threading;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;
using osu.Framework.Platform;

namespace GamesToGo.Desktop.Project
{
    public class Image
    {
        public Texture Texture => texture.Value;
        private readonly Lazy<Texture> texture;

        public readonly string ImageName;

        private TextureStore textures;
        private Storage store;

        private Image(string imageName)
        {
            texture = new Lazy<Texture>(getTexture, LazyThreadSafetyMode.ExecutionAndPublication);
            ImageName = imageName;
        }

        public Image(TextureStore textures, string imageName) : this(imageName)
        {
            this.textures = textures;
        }

        public Image(Storage store, string imageName) : this(imageName)
        {
            this.store = store;
        }

        private Texture getTexture()
        {
            if (textures != null)
                return textures.Get(ImageName);
            else
                return Texture.FromStream(store.GetStream($"files/{ImageName}"));
        }
    }
}
