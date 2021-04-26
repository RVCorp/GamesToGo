using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class PlayerCardsWithTokenArgument : Argument
    {
        public override int ArgumentTypeID => 17;

        public override ArgumentReturnType Type => ArgumentReturnType.MultipleCard;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.SinglePlayer,
            ArgumentReturnType.TokenType,
        };

        public override string[] Text { get; } = {
            @"cartas de",
            @"con fichas de tipo",
        };
    }
}
