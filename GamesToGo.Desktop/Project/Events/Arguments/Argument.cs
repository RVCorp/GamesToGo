using System.Text;
using GamesToGo.Desktop.Project.Elements;
using osu.Framework.Bindables;

namespace GamesToGo.Desktop.Project.Events.Arguments
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

        public abstract ArgumentType Type { get; }

        public abstract bool HasResult { get; }

        public abstract ArgumentType[] ExpectedArguments { get; }

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

        private int? result;

        public int? Result
        {
            get => result;
            set => result = HasResult ? value : null;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            //Tipo de argumento
            builder.Append(ArgumentTypeID);

            //Argumentos o resultado
            builder.Append('(');
            if (!HasResult)
            {
                int argIndex = 0;
                while (argIndex < Arguments.Length)
                {
                    builder.Append(Arguments[argIndex].Value);

                    if (++argIndex < Arguments.Length)
                        builder.Append(',');
                }
            }
            else
            {
                builder.Append(Result);
            }
            builder.Append(')');

            return builder.ToString();
        }
    }
}
