namespace GamesToGo.Desktop.Project.Events
{
    public class SetCardTypeOnTileEvent : ProjectEvent
    {
        public override int TypeID => 2;

        public override EventSourceActivator Source => EventSourceActivator.SingleTile;

        public override EventSourceActivator Activator => EventSourceActivator.SingleCard;

        public override string[] Text => new[] { @"Al colocar una carta de tipo" };

        public override ArgumentType[] ExpectedArguments => new[]
        {
            ArgumentType.CardType,
        };
    }
}
