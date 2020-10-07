using System;
using GamesToGo.Desktop.Project.Events;

namespace GamesToGo.Desktop.Project.Elements
{
    public class DefaultArgument : Argument
    {
        public override int ArgumentTypeID => 0;

        public override ArgumentType Type => ArgumentType.Default;
        public override bool HasResult => true;
        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();
        public override string[] Text { get; } = { @"Añade un argumento aquí" };
    }
}
