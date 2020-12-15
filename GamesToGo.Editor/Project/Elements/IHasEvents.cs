using System.Linq;
using System.Text;
using GamesToGo.Editor.Project.Events;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;

namespace GamesToGo.Editor.Project.Elements
{
    public interface IHasEvents
    {
        BindableList<ProjectEvent> Events { get; }

        public string ToSaveable()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"Events={Events.Count}");
            foreach (var e in Events)
            {
                builder.AppendLine(e.ToString());
            }

            return builder.ToString().TrimEnd('\n', '\r');
        }

        public bool HasReferenceTo(object element)
        {
            return Events.Any(elementEvent => elementEvent.HasReferenceTo(element));
        }

        public void DeleteReferenceTo(object element)
        {
            Events.ForEach(e => e.DeleteReferenceTo(element));
        }
    }
}
