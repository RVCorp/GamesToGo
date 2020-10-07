using System.Collections.Generic;
using System.Text;
using GamesToGo.Desktop.Project.Elements;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Textures;

namespace GamesToGo.Desktop.Project
{
    public abstract class ProjectElement
    {
        public int ID { get; set; }

        public abstract ElementType Type { get; }

        public abstract Bindable<string> Name { get; }

        public abstract Bindable<string> Description { get; }

        protected abstract string DefaultImageName { get; }

        public Image DefaultImage => new Image(Textures, "Elements/" + DefaultImageName, false);

        public abstract Dictionary<string, Bindable<Image>> Images { get; }

        public static TextureStore Textures { protected get; set; }

        public string ToSaveableString()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"{(int)Type}|{ID}|{Name}");
            builder.AppendLine($"Desc={Description}");
            builder.AppendLine($"Images={Images.Count}");

            foreach (var image in Images)
            {
                builder.AppendLine($"{image.Key}={image.Value.Value?.ImageName ?? "null"}");
            }

            if (this is IHasSize sizedElement)
            {
                builder.AppendLine(sizedElement.ToSaveable());
            }

            if (this is IHasPrivacy privacySetElement)
            {
                builder.AppendLine(privacySetElement.ToSaveable());
            }

            if (this is IHasOrientation orientedElement)
            {
                builder.AppendLine(orientedElement.ToSaveable());
            }

            if (this is IHasElements elementedElement)
            {
                builder.AppendLine(elementedElement.ToSaveable());
            }

            if (this is IHasEvents eventedElement)
            {
                builder.AppendLine(eventedElement.ToSaveable());
            }

            return builder.ToString();
        }
    }
}
