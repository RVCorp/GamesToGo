using osu.Framework.Bindables;
using osuTK;

namespace GamesToGo.Editor.Project.Elements
{
    public interface IHasSize
    {
        Bindable<Vector2> Size { get; }

        public string ToSaveable()
        {
            return $"Size={Size.Value.X}|{Size.Value.Y}";
        }
    }
}
