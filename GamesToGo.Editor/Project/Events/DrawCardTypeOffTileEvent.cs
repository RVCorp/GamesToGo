using GamesToGo.Common.Game;
using GamesToGo.Editor.Project.Arguments;
using JetBrains.Annotations;

namespace GamesToGo.Editor.Project.Events
{
    [UsedImplicitly]
    public class DrawCardTypeOffTileEvent : ProjectEvent
    {
        public override int TypeID => 4;

        public override EventSourceActivator Source => EventSourceActivator.SingleTile;

        public override EventSourceActivator Activator => EventSourceActivator.SinglePlayer;

        public override string[] Text => new[] { @"Al tomar una carta de tipo" };

        public override ArgumentReturnType[] ExpectedArguments => new[]
        {
            ArgumentReturnType.CardType,
        };
    }
}
