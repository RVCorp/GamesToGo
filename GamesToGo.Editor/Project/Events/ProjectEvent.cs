using System.Linq;
using System.Text;
using GamesToGo.Common.Game;
using GamesToGo.Editor.Project.Actions;
using GamesToGo.Editor.Project.Arguments;
using osu.Framework.Bindables;

namespace GamesToGo.Editor.Project.Events
{
    public abstract class ProjectEvent
    {
        public int ID { get; set; }

        public abstract int TypeID { get; }

        public abstract EventSourceActivator Source { get; }

        public abstract EventSourceActivator Activator { get; }

        public abstract string[] Text { get; }

        public Bindable<string> Name { get; } = new Bindable<string>();

        public abstract ArgumentReturnType[] ExpectedArguments { get; }

        private Bindable<Argument>[] arguments;

        public Bindable<Argument>[] Arguments
        {
            get
            {
                if (arguments != null)
                    return arguments;

                arguments = new Bindable<Argument>[ExpectedArguments.Length];

                for(int i = 0; i < arguments.Length; i++)
                {
                    arguments[i] = new Bindable<Argument>(new DefaultArgument());
                }

                return arguments;
            }
        }

        public Bindable<int> Priority { get; } = new BindableInt();

        public Bindable<Argument> Condition { get; } = new Bindable<Argument>();

        public BindableList<EventAction> Actions { get; } = new BindableList<EventAction>();

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
                    builder.Append(Arguments[argIndex].Value);

                    if (++argIndex < Arguments.Length)
                        builder.Append(',');
                }
                builder.Append(')');
            }

            builder.Append($"|{Name.Value}|{Priority}|{Condition.Value?.ToString() ?? "null"}|{Actions.Count}");

            foreach(var action in Actions)
            {
                builder.AppendLine();
                builder.Append(action);
            }

            return builder.ToString();
        }

        public bool HasReferenceTo(object reference)
        {
            if (Condition.Value?.HasReferenceTo(reference) ?? false)
                return true;

            //Primero revisar argumentos antes de revisar tremenda lista de acciones
            return Arguments.Any(argument => argument.Value.HasReferenceTo(reference)) ||
                   //No encontramos nada, ahora si a revisar la lista de acciones
                   Actions.Any(action => action.HasReferenceTo(reference));
        }

        public void DeleteReferenceTo(object reference)
        {
            Condition.Value?.DeleteReferenceTo(reference);

            foreach (var argument in Arguments)
                argument.Value.DeleteReferenceTo(reference);

            foreach (var action in Actions)
                action.DeleteReferenceTo(reference);
        }
    }
}
