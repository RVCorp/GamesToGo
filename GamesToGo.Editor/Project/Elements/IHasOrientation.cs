using osu.Framework.Bindables;

namespace GamesToGo.Editor.Project.Elements
{
    public interface IHasOrientation
    {
        Bindable<ElementOrientation> DefaultOrientation { get; }

        public string ToSaveable()
        {
            return $"Orient={DefaultOrientation.Value}";
        }
    }
}
