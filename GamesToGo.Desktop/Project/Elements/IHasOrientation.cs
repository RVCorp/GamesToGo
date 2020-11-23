using osu.Framework.Bindables;

namespace GamesToGo.Desktop.Project.Elements
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
