using System.Collections.Generic;
using System.Text;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Textures;

namespace GamesToGo.Desktop.Project
{
    public abstract class ProjectElement
    {
        public int ID { get; set; }

        public abstract Bindable<string> Name { get; set; }

        protected abstract string DefaultImageName { get; }

        public readonly Image DefaultImage;

        public abstract Dictionary<string, Bindable<Image>> Images { get; }

        public static TextureStore Textures { protected get; set; }

        public ProjectElement()
        {
            DefaultImage = new Image(Textures, "Elements/" + DefaultImageName);
        }

        public virtual string ToSaveable()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"{ID}|{Name}");
            builder.AppendLine($"Images={Images.Count}");
            foreach (var image in Images)
            {
                builder.AppendLine($"{image.Key}={image.Value.Value?.ImageName ?? "null"}");
            }

            return builder.ToString();
        }
    }
}
