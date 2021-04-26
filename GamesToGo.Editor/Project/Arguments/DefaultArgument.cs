using System;
using GamesToGo.Common.Game;

namespace GamesToGo.Editor.Project.Arguments
{
    public class DefaultArgument : Argument
    {
        public override int ArgumentTypeID => 0;

        public override ArgumentReturnType Type => ArgumentReturnType.Default;
        public override ArgumentReturnType[] ExpectedArguments => Array.Empty<ArgumentReturnType>();
        public override string[] Text { get; } = { @"Añade un argumento aquí" };
    }
}
