using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GamesToGo.Editor.Project.Elements;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Textures;

namespace GamesToGo.Editor.Project
{
    public abstract class ProjectElement
    {
        private ProjectElement parent;

        public ProjectElement Parent
        {
            get => parent;
            set
            {
                var parentName = value.Name.Value;

                if (!(value is IHasElements elementedParent))
                    throw new InvalidOperationException($"Can't add element {Name.Value} as children of {parentName} when {parentName} is not {nameof(IHasElements)}");

                if (elementedParent.Elements.All(e => e.ID != ID) && elementedParent.PendingElements.All(i => i != ID))
                    throw new InvalidOperationException($"Can't add element {Name.Value} as children of {parentName} when no children of {parentName} has our id ({ID})");

                parent = value;
            }
        }

        public virtual ElementPreviewMode PreviewMode => ElementPreviewMode.None;

        public int ID { get; set; }

        public abstract ElementType Type { get; }

        public abstract Bindable<string> Name { get; }

        public abstract Bindable<string> Description { get; }

        protected abstract string DefaultImageName { get; }

        private Image defaultImage => new Image(Textures, "Elements/" + DefaultImageName, false);

        public Image GetImageWithFallback(string imageName = null)
        {
            imageName ??= Images.Keys.First();

            if (Images.ContainsKey(imageName) && Images[imageName].Value != null)
                return Images[imageName].Value;

            return defaultImage;
        }

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
                builder.AppendLine($"{image.Key}:{image.Value.Value?.ImageName ?? "null"}");
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

            if (this is IHasSideVisible sidedElement)
            {
                builder.AppendLine(sidedElement.ToSaveable());
            }

            if (this is IHasPosition positionedElement)
            {
                builder.AppendLine(positionedElement.ToSaveable());
            }

            if (this is IHasElements elementedElement)
            {
                builder.AppendLine(elementedElement.ToSaveable());
            }

            if (this is IHasLogicalArrangement arrangedElement)
            {
                builder.AppendLine(arrangedElement.ToSaveable());
            }

            if (this is IHasEvents eventedElement)
            {
                builder.AppendLine(eventedElement.ToSaveable());
            }

            return builder.ToString();
        }
    }
}
