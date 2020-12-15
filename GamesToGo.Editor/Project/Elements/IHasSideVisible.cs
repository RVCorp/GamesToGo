using osu.Framework.Bindables;

namespace GamesToGo.Editor.Project.Elements
{
    public interface IHasSideVisible
    {
        public Bindable<ElementSideVisible> DefaultSide { get; }

        public string ToSaveable()
        {
            return $"Side={DefaultSide.Value}";
        }
    }
}
