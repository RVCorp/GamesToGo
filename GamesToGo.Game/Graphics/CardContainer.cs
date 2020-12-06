using GamesToGo.Game.Online.Models.OnlineProjectElements;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace GamesToGo.Game.Graphics
{
    public class CardContainer : Container
    {
        private Card card;

        public CardContainer(Card card)
        {
            this.card = card;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            //FillAspectRatio = ;
            FillMode = FillMode.Fit;
        }
    }
}
