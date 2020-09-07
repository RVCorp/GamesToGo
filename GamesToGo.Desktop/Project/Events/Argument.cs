using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
{
    public abstract class Argument
    {
        public abstract int ArgumentTypeID { get; }

        public abstract ArgumentType Type { get; }

        public abstract bool HasResult { get; }

        public abstract IEnumerable<ArgumentType> ExpectedArguments { get; }

        public Argument[] Arguments { get; }

        private int? result = null;

        public int? Result
        {
            get => HasResult ? result : result;
            set => result = HasResult ? value : null;
        }

        public Argument()
        {
            Arguments = new Argument[ExpectedArguments.Count()];
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
                    builder.Append(Arguments[argIndex].ToString());

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
