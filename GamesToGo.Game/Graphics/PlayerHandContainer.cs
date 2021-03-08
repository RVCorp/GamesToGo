using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GamesToGo.Common.Online;
using GamesToGo.Common.Online.RequestModel;
using GamesToGo.Game.LocalGame;
using GamesToGo.Game.LocalGame.Elements;
using GamesToGo.Game.Online.Models.OnlineProjectElements;
using GamesToGo.Game.Online.Models.RequestModel;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
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
        private readonly Bindable<Card> currentSelectedCard = new Bindable<Card>();
        [Resolved]
        private Bindable<OnlineRoom> room { get; set; }
        [Resolved]
        private APIController api { get; set; }

        private BindableList<OnlineCard> localCards = new BindableList<OnlineCard>();
        public IBindable<Card> CurrentSelectedCard => currentSelectedCard;

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
            for (int i = 0; i < localCards.Count; i++)
            {
                if (cards.All(c => c.ID != localCards[i].ID))
                    continue;

                cards.Remove(cards.First(c => c.ID == localCards[i].ID));
                localCards.Remove(localCards[i]);
                i--;
            }
            if (localCards.Any() || cards.Any())
            {
                localCards.AddRange(cards.Select(c => new OnlineCard
                {
                    ID = c.ID,
                    TypeID = c.TypeID,
                    Orientation = c.Orientation,
                    FrontVisible = c.FrontVisible,
                    Tokens = c.Tokens
                }));

                foreach (var card in localCards)
                {
                    playerCards.Add(new CardContainer(card));
                }

                foreach (var oldCard in localCards)
                {
                    localCards.Remove(localCards.First(s => s.ID == oldCard.ID));
                    playerCards.RemoveRange(playerCards.Where(c => c.Card.ID == oldCard.ID));
                }

            }
        }

        public void SelectCard(Card card)
        {
            if (currentSelectedCard.Value == card)
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
