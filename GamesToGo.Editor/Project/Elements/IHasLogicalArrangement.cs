using osu.Framework.Bindables;
using osuTK;

namespace GamesToGo.Editor.Project.Elements
{
    public interface IHasLogicalArrangement
    {
        Bindable<Vector2> Arrangement { get; }

        public string ToSaveable()
        {
            return $"Arrangement={Arrangement.Value.X}|{Arrangement.Value.Y}";
        }
    }
}
