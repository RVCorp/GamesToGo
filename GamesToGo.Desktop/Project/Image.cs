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

        public Image(Func<Texture> func, File databaseObject)
        {
            Texture = new Lazy<Texture>(func, LazyThreadSafetyMode.ExecutionAndPublication);
            DatabaseObject = databaseObject;
        }

    }
}
