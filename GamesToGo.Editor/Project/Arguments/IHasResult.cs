using osu.Framework.Bindables;

namespace GamesToGo.Editor.Project.Arguments
{
    public interface IHasResult
    {
        Bindable<int?> Result { get; }

        bool ResultMapsTo(object result);
    }
}
