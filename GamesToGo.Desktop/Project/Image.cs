using System;
using System.IO;
using System.Threading;
using osu.Framework.Graphics.Textures;

namespace GamesToGo.Desktop.Project
{
    public class Image
    {
        public Texture Texture => texture.Value;

        private readonly bool fromStorage;
        private readonly Lazy<Texture> texture;

        public readonly string ImageName;

        private readonly TextureStore textures;

        private Image(string imageName, bool fromStorage)
        {
            this.fromStorage = fromStorage;
            texture = new Lazy<Texture>(getTexture, LazyThreadSafetyMode.ExecutionAndPublication);
            ImageName = imageName;
        }

        public Image(TextureStore textures, string imageName, bool fromStorage = true) : this(imageName, fromStorage)
        {
            this.textures = textures;
        }

        private Texture getTexture() => textures.Get(fromStorage ? Path.Combine("files", ImageName) : ImageName);
    }
}
