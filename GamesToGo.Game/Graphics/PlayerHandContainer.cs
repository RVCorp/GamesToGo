using System.Collections.Generic;
using System.Linq;
using GamesToGo.Common.Online;
using GamesToGo.Game.LocalGame;
using GamesToGo.Game.Online.Models.OnlineProjectElements;
using GamesToGo.Game.Online.Models.RequestModel;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace GamesToGo.Game.Graphics
{
    [Cached]
    public class PlayerHandContainer : Container
    {
        private FillFlowContainer<CardContainer> playerCards;
        [Resolved]
        private WorkingGame game { get; set; }

        private BasicScrollContainer scroll;
        private HelpContainer description;
        private readonly Bindable<OnlineCard> currentSelectedCard = new Bindable<OnlineCard>();
        [Resolved]
        private Bindable<OnlineRoom> room { get; set; }
        [Resolved]
        private APIController api { get; set; }

        private readonly BindableList<OnlineCard> localCards = new BindableList<OnlineCard>();
        public IBindable<OnlineCard> CurrentSelectedCard => currentSelectedCard;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Black.Opacity(0.6f)
                },
                scroll = new BasicScrollContainer(Direction.Horizontal)
                {
                    RelativeSizeAxes = Axes.Both,
                    ScrollbarOverlapsContent = false,
                    Child = playerCards = new FillFlowContainer<CardContainer>
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Y,
                        AutoSizeAxes = Axes.X,
                        Direction = FillDirection.Horizontal,
                    },
                },
                description = new HelpContainer(80)
                {

                }
            };
            description.Hide();
            scroll.ScrollContent.Anchor = Anchor.Centre;
            scroll.ScrollContent.Origin = Anchor.Centre;
            room.BindValueChanged(updatedRoom => checkPlayerHand(updatedRoom.NewValue.PlayerWithID(api.LocalUser.Value.ID).Hand.Cards), true);
        }

        private void checkPlayerHand(ICollection<OnlineCard> cards)
        {
            playerCards.RemoveRange(playerCards.Where(c => cards.All(oc => oc.ID != c.Model.ID)));

            var toBeAddedCards = cards.Where(oc => playerCards.All(c => c.Model.ID != oc.ID)).ToList();

            foreach (var card in cards.Except(toBeAddedCards))
                playerCards.Single(c => c.Model.ID == card.ID).Model = card;

            foreach (var card in toBeAddedCards)
            {
                playerCards.Add(new CardContainer { Model = card });
            }
        }

        public void SelectCard(OnlineCard card)
        {
            if (Equals(currentSelectedCard.Value, card))
                currentSelectedCard.Value = null;
            else
                currentSelectedCard.Value = card;
        }
        public void ShowDescription(string desc, Vector2 pos)
        {
            description.Text = desc;
            description.Pos = pos;
            description.Show();
        }
        public void HideDescription()
        {
            description.Hide();
        }
    }
}
