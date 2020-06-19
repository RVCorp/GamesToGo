using System;
using System.Threading;
using GamesToGo.Desktop.Database.Models;
using osu.Framework.Graphics.Textures;

namespace GamesToGo.Desktop.Project
{
    public class Image
    {
        public readonly Lazy<Texture> Texture;

        public readonly File DatabaseObject;

        private TextureStore textures;

        public Image(TextureStore textures, File databaseObject)
        {
            this.textures = textures;
            Texture = new Lazy<Texture>(getTexture, LazyThreadSafetyMode.ExecutionAndPublication);
            DatabaseObject = databaseObject;
        }

        private Texture getTexture()
        {
            return textures.Get($"files/{DatabaseObject.NewName}");
        }
    }
}
