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
        private readonly Tile tile;
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

        private readonly IBindable<Tile> currentSelected = new Bindable<Tile>();
        private bool selected => (currentSelected.Value?.TypeID ?? -1) == tile.TypeID;
        private Container borderContainer;

        public TileContainer(Tile tile)
        {
            this.tile = tile;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Action += () => boards.SelectTile(tile);
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
                tileImage = new ContainedImage(false, 0)
                {
                    BorderThickness = .3f,
                    BorderColour = Colour4.White,
                    RelativeSizeAxes = Axes.Both,
                    Texture = tile.Images.First(),
                    ImageSize = tile.Size,
                },
            };

            tileImage.Rotation = (int)tile.Orientation * -90;
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

            /*Tile.Tokens.Clear(); 
            tileImage.OverImageContent.RemoveRange(tileImage.OverImageContent.Where(t => t is TokenContainer)); 
            foreach (var token in updatedTile.Tokens) 
            { 
                Token newToken; 
                Tile.Tokens.Add(newToken = new Token 
                { 
                    ID = token.TypeID, 
                    Amount = token.Count 
                }); 
                tileImage.OverImageContent.Add(new TokenContainer(newToken)); 
            }*/
        }
    }
}
