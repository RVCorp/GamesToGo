using System;
using System.Text;
using GamesToGo.Desktop.Project.Events;
using osu.Framework.Bindables;

namespace GamesToGo.Desktop.Project.Elements
{
    public interface IHasEvents
    {
        BindableList<Event> Events { get; }

        Type[] CompatibleEvents { get; }

        public string ToSaveable()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"Events={Events.Count}");
            foreach (var evnt in Events)
            {
                builder.AppendLine(evnt.ToString());
            }

            return builder.ToString().TrimEnd('\n', '\r');
        }
    }
}
