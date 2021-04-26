using GamesToGo.Common.Game;
using GamesToGo.Editor.Project.Arguments;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class GiveXCardsATokenAction : EventAction
    {
        public override int TypeID => 14;

        public override ArgumentReturnType[] ExpectedArguments { get; } = {
            ArgumentReturnType.TokenType,
            ArgumentReturnType.SingleNumber,
            ArgumentReturnType.SingleTile,
        };

        public override string[] Text { get; } = {
            @"Dar ficha",
            @"a las primeras",
            @"cartas de la casilla",
        };
    }
}
