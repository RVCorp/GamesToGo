using System.Collections.Generic;
using System.Linq;
using GamesToGo.Game.LocalGame.Elements;
using GamesToGo.Game.Online.Models.OnlineProjectElements;
using GamesToGo.Game.Online.Models.RequestModel;
using GamesToGo.Game.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Testing;

namespace GamesToGo.Game.Graphics
{
    public class TileContainer : Button
    {
        public readonly Tile Tile;
        private OnlineTile model;

        public OnlineTile Model
        {
            get => model;
            set
            {
                model = value;
                checkTile();
            }
        }

        private ContainedImage tileImage;

        private List<CardContainer> cards => tileImage.OverImageContent.ChildrenOfType<CardContainer>().ToList();

        public List<Token> Tokens { get; } = new List<Token>();

        [Resolved]
        private BoardsContainer boards { get; set; }

        [Resolved]
        private Bindable<OnlineRoom> room { get; set; }
        [Resolved]
        private GameScreen gameScreen { get; set; }

        private readonly IBindable<OnlineTile> currentSelected = new Bindable<OnlineTile>();
        private bool selected => (currentSelected.Value?.TypeID ?? -1) == Tile.TypeID;
        private Container borderContainer;
        private FillFlowContainer<TokenContainer> tileTokens;

        public TileContainer(Tile tile)
        {
            Tile = tile;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Action += () => boards.SelectTile(model);
            Enabled.BindTo(gameScreen.EnableTileSelection);
            currentSelected.BindTo(boards.CurrentSelectedTile);
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
                        tileImage = new ContainedImage(false, 0)
                        {
                            BorderThickness = .3f,
                            BorderColour = Colour4.White,
                            RelativeSizeAxes = Axes.Both,
                            Height = .7f, 
                            Texture = Tile.Images.FirstOrDefault(),
                            ImageSize = Tile.Size,
                        },
                        new BasicScrollContainer(Direction.Horizontal)
                        {
                            RelativeSizeAxes = Axes.Both,
                            Width = .3f,
                            ScrollbarOverlapsContent = false,
                            Child = tileTokens = new FillFlowContainer<TokenContainer>
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

            tileImage.Rotation = (int)Tile.Orientation * -90;
            tileImage.OverImageContent.Clear();
            currentSelected.BindValueChanged(_ =>
            {
                if (Enabled.Value == true)
                    FadeBorder(selected || IsHovered, golden: selected);
            });
        }

        protected void FadeBorder(bool visible, bool instant = false, bool golden = false)
        {
            borderContainer.Colour = golden ? Colour4.Gold : Colour4.White;
        }

        private void checkTile()
        {
            checkCards();
            checkTokens();
        }

        private void checkCards()
        {
            tileImage.OverImageContent.RemoveRange(cards.Where(c => model.Cards.All(oc => oc.ID != c.Model.ID)));

            var toBeAddedCards = model.Cards.Where(oc => cards.All(c => c.Model.ID != oc.ID)).ToList();

            foreach (var card in model.Cards.Except(toBeAddedCards))
                this.ChildrenOfType<CardContainer>().Single(c => c.Model.ID == card.ID).Model = card;

            foreach (var card in toBeAddedCards)
            {
                tileImage.OverImageContent.Add(new CardContainer()
                {
                    Model = card,
                });
            }
        }

        private void checkTokens()
        {
            tileTokens.Clear();

            foreach (var token in model.Tokens)
            {
                tileTokens.Add(new TokenContainer { Model = token });
            }
        }
    }
}
