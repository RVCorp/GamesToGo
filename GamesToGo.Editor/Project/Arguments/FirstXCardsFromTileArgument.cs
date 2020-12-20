using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Arguments
{
    [UsedImplicitly]
    public class FirstXCardsFromTileArgument : Argument
    {
        public override int ArgumentTypeID => 16;

        public override ArgumentType Type => ArgumentType.MultipleCard;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.SingleNumber,
            ArgumentType.TileType,
        };

        public override string[] Text { get; } = {
            @"primeras",
            @"cartas de",
        };
    }
}
