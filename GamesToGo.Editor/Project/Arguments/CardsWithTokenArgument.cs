using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class CardsWithTokenArgument : Argument
    {
        public override int ArgumentTypeID => 18;

        public override ArgumentReturnType Type => ArgumentReturnType.MultipleCard;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.TokenType,
        };

        public override string[] Text { get; } = {
            @"cartas con fichas",
        };
    }
}
