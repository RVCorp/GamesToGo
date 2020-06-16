using osu.Framework.Bindables;

namespace GamesToGo.Desktop.Project.Elements
{
    public interface IHasEvents
    {
        BindableList<int> Events { get; }
    }
}
