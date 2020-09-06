using System.Collections.Generic;
using System.Linq;
using System.Text;
using osu.Framework.Bindables;

namespace GamesToGo.Desktop.Project.Events
{
    public abstract class Event
    {
        public int ID { get; set; }

        public abstract int TypeID { get; }

        public abstract EventSourceActivator Source { get; }

        public abstract EventSourceActivator Activator { get; }

        public abstract IEnumerable<string> Text { get; }

        public abstract IEnumerable<ArgumentType> ExpectedArguments { get; }

        public Argument[] Arguments { get; }

        public int Priority { get; set; }

        public Argument Condition { get; set; } = null;

        public BindableList<EventAction> Actions { get; } = new BindableList<EventAction>();

        public Event()
        {
            Arguments = new Argument[ExpectedArguments.Count()];
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append($"{ID}|{TypeID}");

            if (ExpectedArguments != null)
            {
                builder.Append('(');
                int argIndex = 0;
                while (argIndex < Arguments.Length)
                {
                    builder.Append(Arguments[argIndex].ToString());

                    if (++argIndex < Arguments.Length)
                        builder.Append(',');
                }
                builder.Append(')');
            }

            builder.Append($"|{Priority}");

            if (Condition != null)
            {
                builder.Append('|');
                builder.Append(Condition.ToString());
            }

            builder.AppendLine();

            foreach(var action in Actions)
            {
                builder.AppendLine(action.ToString());
            }

            return base.ToString();
        }
    }
}
