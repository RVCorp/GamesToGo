using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GamesToGo.Desktop.Project.Elements;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Textures;

namespace GamesToGo.Desktop.Project
{
    public abstract class ProjectElement
    {
        private ProjectElement parent;

        public ProjectElement Parent
        {
            get => parent;
            set
            {
                if (!(value is IHasElements elementedParent))
                {
                    var parentName = value.Name.Value;
                    throw new InvalidOperationException($"Can't add element {Name.Value} as children of {parentName} when {parentName} is not {nameof(IHasElements)}");
                }

                if (!(elementedParent.Elements.Any(e => e.ID == ID) ||
                      elementedParent.PendingElements.Any(i => i == ID)))
                {
                    var parentName = value.Name.Value;
                    throw new InvalidOperationException($"Can't add element {Name.Value} as children of {parentName} when no children of {parentName} has our id ({ID})");
                }

                parent = value;
            }
        }

        public virtual ElementPreviewMode PreviewMode => ElementPreviewMode.None;

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

            if(this is IHasPosition positionedElement)
            {
                builder.AppendLine(positionedElement.ToSaveable());
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
