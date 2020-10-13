using System.Text;
using GamesToGo.Desktop.Project.Arguments;
using osu.Framework.Bindables;

namespace GamesToGo.Desktop.Project.Actions
{
    public abstract class EventAction
    {
        public abstract int TypeID { get; }

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

        public Argument Condition { get; set; } = null;

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

            if (Condition != null)
            {
                builder.Append('|');
                builder.Append(Condition);
            }

            return builder.ToString();
        }
    }
}
