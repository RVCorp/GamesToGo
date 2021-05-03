using GamesToGo.Common.Game;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class FirstXCardsFromTileArgument : Argument
    {
        public override int ArgumentTypeID => 16;

        public override ArgumentReturnType Type => ArgumentReturnType.MultipleCard;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.SingleNumber,
            ArgumentReturnType.TileType,
        };

        public override string[] Text { get; } = {
            @"primeras",
            @"cartas de",
        };
    }
}
