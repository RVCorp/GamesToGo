using System.Linq;
using System.Text;
using GamesToGo.Common.Game;
using GamesToGo.Editor.Project.Arguments;
using osu.Framework.Bindables;

namespace GamesToGo.Editor.Project.Actions
{
    public abstract class EventAction
    {
        public abstract int TypeID { get; }

        public abstract ArgumentReturnType[] ExpectedArguments { get; }

        private Bindable<Argument>[] arguments;

        public Bindable<Argument>[] Arguments
        {
            get
            {
                if (arguments != null)
                    return arguments;

                arguments = new Bindable<Argument>[ExpectedArguments.Length];

                for (int i = 0; i < arguments.Length; i++)
                {
                    arguments[i] = new Bindable<Argument>(new DefaultArgument());
                }

                return arguments;
            }
        }

        public abstract string[] Text { get; }

        public Bindable<Argument> Condition { get; } = new Bindable<Argument>();

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append('|');
            //Tipo de accion
            builder.Append(TypeID);

            //Argumentos o resultado
            builder.Append('(');

            if (ExpectedArguments != null)
            {
                int argIndex = 0;

                while (argIndex < Arguments.Length)
                {
                    builder.Append(Arguments[argIndex].Value);

                    if (++argIndex < Arguments.Length)
                        builder.Append(',');
                }
            }

            builder.Append(')');

            builder.Append('|');
            builder.Append(Condition.Value?.ToString() ?? "null");

            return builder.ToString();
        }

        public bool HasReferenceTo(object reference)
        {
            if (Condition.Value?.HasReferenceTo(reference) ?? false)
                return true;

            return Arguments.Any(argument => argument.Value.HasReferenceTo(reference));
        }

        public void DeleteReferenceTo(object reference)
        {
            Condition.Value?.DeleteReferenceTo(reference);

            foreach (var argument in Arguments)
                argument.Value.DeleteReferenceTo(reference);
        }
    }
}
