using System.Linq;
using System.Text;
using GamesToGo.Common.Game;
using osu.Framework.Bindables;

namespace GamesToGo.Editor.Project.Arguments
{
    /// <summary>
    /// ActionDescriptor(ctor: Action)
    /// -ArgumentChanger(ctor: ArgumentType)
    /// --BindableArgument
    /// --ArgumentDescriptor(ctor: Argument)
    /// </summary>
    public abstract class Argument
    {
        public abstract int ArgumentTypeID { get; }

        public abstract ArgumentReturnType Type { get; }

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

        public abstract string[] Text { get; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            //Tipo de argumento
            builder.Append(ArgumentTypeID);

            //Argumentos o resultado
            builder.Append('(');

            if (this is IHasResult resolvedArgument)
            {
                builder.Append(resolvedArgument.Result.Value?.ToString() ?? string.Empty);
            }
            else
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

            return builder.ToString();
        }

        public void DeleteReferenceTo(object reference)
        {
            if (this is IHasResult resolvedArgument && resolvedArgument.ResultMapsTo(reference))
                resolvedArgument.Result.Value = null;

            foreach (var arg in Arguments)
            {
                arg.Value?.DeleteReferenceTo(reference);
            }
        }

        public bool HasReferenceTo(object reference)
        {
            if (this is IHasResult resolvedArgument && resolvedArgument.ResultMapsTo(reference))
                return true;

            return Arguments.Any(subArg => subArg.Value.HasReferenceTo(reference));
        }
    }
}
