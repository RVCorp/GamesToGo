using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GamesToGo.Desktop.Project.Elements
{
    public interface IHasElements
    {
        public void QueueSubelement(int ID)
        {
            PendingSubelements.Enqueue(ID);
        }

        public Queue<int> PendingSubelements { get; }

        List<ProjectElement> Subelements { get; }

        ElementType SubelementType { get; }

        public string ToSaveable()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"SubElems={Subelements.Count}");
            foreach (var element in Subelements.Select(e => e.ID).ToList())
            {
                builder.AppendLine($"{element}");
            }

            return builder.ToString();
        }
    }
}
