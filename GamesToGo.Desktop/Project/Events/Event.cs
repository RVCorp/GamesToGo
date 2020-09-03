using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
{
    public abstract class Event
    {
        public int ID { get; set; }

        public abstract int TypeID { get; }

        public abstract EventType Type { get; }

        public abstract IEnumerable<string> Text { get; }

        public abstract IEnumerable<ArgumentType> ExpectedArguments { get; }

        public Argument[] Arguments { get; }

        public int Priority { get; set; }

        public Argument Condition { get; set; } = null;

        public List<EventAction> Actions { get; } = new List<EventAction>();

        public Event()
        {
            Arguments = new Argument[ExpectedArguments.Count()];
        }
    }
}
