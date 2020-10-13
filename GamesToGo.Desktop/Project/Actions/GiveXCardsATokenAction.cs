using GamesToGo.Desktop.Project.Arguments;

namespace GamesToGo.Desktop.Project.Actions
{
    public class GiveXCardsATokenAction : EventAction
    {
        public override int TypeID => 14;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.TokenType,
            ArgumentType.SingleNumber,
            ArgumentType.SingleTile
        };

        public override string[] Text { get; } = {
            @"Dar ficha",
            @"a las primeras",
            @"cartas de la casilla"
        };
    }
}
