using System.Linq;
using GamesToGo.Game.LocalGame;
using GamesToGo.Game.LocalGame.Elements;
using GamesToGo.Game.Online.Models.OnlineProjectElements;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace GamesToGo.Game.Graphics
{
    public class CardContainer : Container
    {
        public Card Card;
        private ContainedImage cardImage;

        [Resolved]
        private WorkingGame game { get; set; }
        public CardContainer(OnlineCard card)
        {
            Card = game.GameCards.FirstOrDefault(c => c.ID == card.TypeID);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Width = .2f;
            Child = cardImage = new ContainedImage(true, 0)
            {
                RelativeSizeAxes = Axes.Both,
                Texture = Card.Images.First(),
                ImageSize = Card.Size
            };
        }
    }
}
