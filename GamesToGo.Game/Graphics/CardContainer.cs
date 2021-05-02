using System.Collections.Generic;
using System.Linq;
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
using osu.Framework.Testing;
using osu.Framework.Threading;
using osuTK;

namespace GamesToGo.Game.Graphics
{
    public class CardContainer : SurfaceButton
    {
        private Card fileCard;

        private OnlineCard model;
        public OnlineCard Model
        {
            get => model;
            set
            {
                model = value;

                CheckCard(model.Tokens);
            }
        }

        private ContainedImage cardFrontImage;
        private ContainedImage cardBackImage; //ToDo: OK
        private Container borderContainer;
        private readonly IBindable<Card> currentSelected = new Bindable<Card>();
        private bool selected => (currentSelected.Value?.TypeID ?? -1) == fileCard.TypeID;
        private ScheduledDelegate delayedShow;
        private FillFlowContainer<TokenContainer> cardTokens;

        [Resolved]
        private PlayerHandContainer hand { get; set; }
        [Resolved]
        private WorkingGame game { get; set; }
        [Resolved]
        private Bindable<OnlineRoom> room { get; set; }
        [Resolved]
        private GameScreen gameScreen { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            fileCard = game.GameCards.First(c => c.TypeID == model.TypeID);
            Enabled.BindTo(gameScreen.EnableCardSelection);
            Action += () => hand.SelectCard(model);
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
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        cardFrontImage = new ContainedImage(false, 0)
                        {
                            RelativeSizeAxes = Axes.Both,
                            Texture = fileCard.Images.First(),
                            ImageSize = fileCard.Size,
                        },
                        new BasicScrollContainer(Direction.Horizontal)
                        {
                            RelativeSizeAxes = Axes.Both,
                            Width = .3f,
                            ScrollbarOverlapsContent = false,
                            Child = cardTokens = new FillFlowContainer<TokenContainer>
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativeSizeAxes = Axes.Y,
                                AutoSizeAxes = Axes.X,
                                Direction = FillDirection.Horizontal,
                            },
                        }
                    }
                }
            };
            currentSelected.BindValueChanged(_ =>
            {
                if (Enabled.Value == true)
                    FadeBorder(selected || IsHovered, golden: selected);
            });
        }

        public void CheckCard(List<OnlineToken> updatedTokens)
        {
            cardTokens.Clear();

            foreach(var token in model.Tokens)
            {
                cardTokens.Add(new TokenContainer { Model = token, Size = new Vector2(20,20)});
            }
        }

        protected void FadeBorder(bool visible, bool instant = false, bool golden = false)
        {
            borderContainer.Colour = golden ? Colour4.Gold : Colour4.White;
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            base.OnMouseDown(e);
            delayedShow = Scheduler.AddDelayed(() => hand.ShowDescription(fileCard.Description, Position), 1400);
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
