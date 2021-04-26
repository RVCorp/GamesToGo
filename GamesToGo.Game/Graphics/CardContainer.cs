using System.Collections.Generic;
using System.Linq;
using GamesToGo.Common.Online;
using GamesToGo.Game.LocalGame;
using GamesToGo.Game.LocalGame.Elements;
using GamesToGo.Game.Online.Models.OnlineProjectElements;
using GamesToGo.Game.Online.Models.RequestModel;
using GamesToGo.Game.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input.Events;
using osu.Framework.Threading;

namespace GamesToGo.Game.Graphics
{
    public class CardContainer : SurfaceButton
    {
        public Card Card;
        private ContainedImage cardFrontImage;
        private ContainedImage cardBackImage; //ToDo
        private Container borderContainer;
        private readonly IBindable<Card> currentSelected = new Bindable<Card>();
        private bool selected => (currentSelected.Value?.ID ?? -1) == Card.ID;
        private ScheduledDelegate delayedShow;
        private TileContainer tile;
        [Resolved]
        private PlayerHandContainer hand {get; set;}
        [Resolved]
        private WorkingGame game { get; set; }
        [Resolved]
        private Bindable<OnlineRoom> room { get; set; }
        [Resolved]
        private GameScreen gameScreen { get; set; }

        public CardContainer(OnlineCard card)
        {
            Card = game.GameCards.Where(c => c.ID == card.ID).FirstOrDefault();
        }

        public CardContainer(OnlineCard card, TileContainer tile)
        {            
            Card = game.GameCards.Where(c => c.ID == card.ID).FirstOrDefault();
            this.tile = tile;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Enabled.BindTo(gameScreen.EnableCardSelection);
            Action += () => hand.SelectCard(Card);
            currentSelected.BindTo(hand.CurrentSelectedCard);
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            RelativeSizeAxes = Axes.Y;
            Width = 200;
            Children = new Drawable[]
            {
                borderContainer = new Container
                {
                    Masking = true,
                    CornerRadius = 10,
                    BorderThickness = 2,
                    BorderColour = Colour4.White,
                    RelativeSizeAxes = Axes.Both,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0.1f,
                    },
                },
                cardFrontImage = new ContainedImage(false, 0)
                {
                    RelativeSizeAxes = Axes.Both,
                    Texture = Card.Images.First(),
                    ImageSize = Card.Size
                }
            };
            currentSelected.BindValueChanged(_ =>
            {
                if (Enabled.Value == true)
                    FadeBorder(selected || IsHovered, golden: selected);
            });
            room.BindValueChanged(updatedRoom => checkCard(updatedRoom.NewValue.Boards.Where(b => b.TypeID == tile.BoardID).FirstOrDefault().Tiles.Where(t => t.TypeID == tile.Tile.ID).FirstOrDefault().Cards.Where(c => c.ID == Card.ID).FirstOrDefault().Tokens), true);
        }

        private void checkCard(List<OnlineToken> updatedTokens)
        {
            Card.Tokens.Clear();
            cardFrontImage.OverImageContent.RemoveRange(cardFrontImage.OverImageContent.Where(t => t is TokenContainer));
            foreach (var token in updatedTokens)
            {
                Token newToken;
                Card.Tokens.Add(newToken = new Token
                {
                    ID = token.TypeID,
                    Amount = token.Count
                });
                cardFrontImage.OverImageContent.Add(new TokenContainer(newToken));
            }
        }

        protected void FadeBorder(bool visible, bool instant = false, bool golden = false)
        {
            borderContainer.Colour = golden ? Colour4.Gold : Colour4.White;
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            base.OnMouseDown(e);
            delayedShow = Scheduler.AddDelayed(() => hand.ShowDescription(Card.Description, Position), 1400);
            return true;
        }

        protected override void OnMouseUp(MouseUpEvent e)
        {
            base.OnMouseUp(e);
            delayedShow?.Cancel();
            delayedShow = null;
            hand.HideDescription();
        }
    }
}
