using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GamesToGo.Desktop.Project.Events
{
    public abstract class EventAction
    {
        public abstract int TypeID { get; }

        public abstract IEnumerable<ArgumentType> ExpectedArguments { get; }

        public Argument[] Arguments { get; }

        public Argument Condition { get; set; } = null;

        public EventAction()
        {
            Arguments = new Argument[ExpectedArguments.Count()];
        }

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
                    builder.Append(Arguments[argIndex].ToString());

                    if (++argIndex < Arguments.Length)
                        builder.Append(',');
                }
            }
            builder.Append(')');

            if (Condition != null)
            {
                builder.Append('|');
                builder.Append(Condition.ToString());
            }

            return builder.ToString();
        }
    }
}
