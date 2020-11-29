using osu.Framework.Bindables;
using osuTK;

namespace GamesToGo.Editor.Project.Elements
{
    public interface IHasPosition
    {
        Bindable<Vector2> Position { get; }

        public string ToSaveable()
        {
            return $"Position={Position.Value.X}|{Position.Value.Y}";
        }
    }
}
