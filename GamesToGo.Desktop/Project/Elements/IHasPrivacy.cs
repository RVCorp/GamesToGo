using osu.Framework.Bindables;

namespace GamesToGo.Desktop.Project.Elements
{
    public interface IHasPrivacy
    {
        Bindable<ElementPrivacy> DefaultPrivacy { get; }

        public string ToSaveable()
        {
            return $"Privacy={DefaultPrivacy.Value}";
        }
    }
}
