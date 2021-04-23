using GamesToGo.Editor.Project.Arguments;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Actions
{
    [UsedImplicitly]
    public class AddCardToTileChosenByPlayer : EventAction
    {
        public override int TypeID => 25;

        public override ArgumentType[] ExpectedArguments { get; } = {
            ArgumentType.CardType,
            ArgumentType.SingleTile,
        };

        public override string[] Text { get; } = {
            @"Añadir nueva carta",
            @"a casilla",
        };
    }
}
