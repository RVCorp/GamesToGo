using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GamesToGo.Game.LocalGame.Elements
{
    public interface IHasElements
    {
        public void QueueElement(int id)
        {
            PendingElements.Enqueue(id);
        }

        public Queue<int> PendingElements { get; }

        List<GameElement> Elements { get; }

        ElementType NestedElementType { get; }

        public string ToSaveable()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"SubElems={Elements.Count}");
            foreach (var element in Elements.Select(e => e.TypeID).ToList())
            {
                builder.AppendLine($"{element}");
            }

            return builder.ToString().TrimEnd('\n', '\r');
        }
    }
}
