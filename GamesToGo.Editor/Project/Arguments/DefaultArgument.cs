using System;

namespace GamesToGo.Editor.Project.Arguments
{
    public class DefaultArgument : Argument
    {
        public override int ArgumentTypeID => 0;

        public override ArgumentType Type => ArgumentType.Default;
        public override bool HasResult => false;
        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();
        public override string[] Text { get; } = { @"Añade un argumento aquí" };
    }
}
