using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GamesToGo.Desktop.Project.Elements
{
    public interface IHasElements
    {
        public void QueueElement(int id)
        {
            PendingElements.Enqueue(id);
        }

        public Queue<int> PendingElements { get; }

        List<ProjectElement> Elements { get; }

        ElementType NestedElementType { get; }

        public string ToSaveable()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"SubElems={Elements.Count}");
            foreach (var element in Elements.Select(e => e.ID).ToList())
            {
                builder.AppendLine($"{element}");
            }

            return builder.ToString().TrimEnd('\n', '\r');
        }
    }
}
