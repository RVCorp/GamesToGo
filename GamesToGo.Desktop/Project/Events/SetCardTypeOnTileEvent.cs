using System.Collections.Generic;

namespace GamesToGo.Desktop.Project.Events
{
    public class SetCardTypeOnTileEvent : Event
    {
        public override int TypeID => 2;

        public override EventType Type => EventType.SingleCard;

        public override IEnumerable<string> Text => new[] { "Al colocar una carta" };

        public override IEnumerable<ArgumentType> ExpectedArguments => new[]
        {
            ArgumentType.CardType,
        };
    }
}
