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
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;

namespace GamesToGo.Game.Graphics
{
    public class TileContainer : Button
    {
        public Tile Tile;

        public ContainedImage TileImage;

        [Resolved]
        private BoardsContainer boards { get; set; }

        [Resolved]
        private Bindable<OnlineRoom> room { get; set; }
        [Resolved]
        private GameScreen gameScreen { get; set; }

        private readonly IBindable<Tile> currentSelected = new Bindable<Tile>();
        private bool selected => (currentSelected.Value?.ID ?? -1) == Tile.ID;
        public int BoardID;
        private Container borderContainer;

        public TileContainer(Tile tile, int boardID)
        {
            this.Tile = tile;
            this.BoardID = boardID;
        }

        [BackgroundDependencyLoader]
        private void load()
        {            
            Action += () => boards.SelectTile(Tile);
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
                TileImage =new ContainedImage(false, 0)
                {
                    BorderThickness = .3f,
                    BorderColour = Colour4.White,
                    RelativeSizeAxes = Axes.Both,
                    Texture = Tile.Images.First(),
                    ImageSize = Tile.Size
                }
            };
                
            TileImage.Rotation = (int)Tile.Orientation * -90;
            TileImage.OverImageContent.Clear();
            currentSelected.BindValueChanged(_ =>
            {
                if (Enabled.Value == true)
                    FadeBorder(selected || IsHovered, golden: selected);
            });
            //room.BindValueChanged(updatedRoom => checkTile(updatedRoom.NewValue.Boards.Where(b => b.TypeID == BoardID).FirstOrDefault().Tiles.Where(t => t.TypeID == Tile.ID).FirstOrDefault()), true);
        }

        protected void FadeBorder(bool visible, bool instant = false, bool golden = false)
        {
            borderContainer.Colour = golden ? Colour4.Gold : Colour4.White;
        }

        private void checkTile(OnlineTile tile)
        {
            for (int j = 0; j < Tile.Cards.Count; j++)
            {
                if (tile.Cards.All(c => c.ID != Tile.Cards[j].ID))
                    continue;

                tile.Cards.Remove(tile.Cards.First(c => c.ID == Tile.Cards[j].ID));
                Tile.Cards.Remove(Tile.Cards[j]);
                j--;
            }
            if (Tile.Cards.Any() || tile.Cards.Any())
            {
                Tile.Cards.AddRange(tile.Cards.Select(c => new Card
                {
                    ID = c.ID,
                    TypeID = c.TypeID,
                    Orientation = c.Orientation,
                    FrontVisible = c.FrontVisible,
                }));

                foreach (var card in Tile.Cards)
                {
                    TileImage.OverImageContent.Add(new CardContainer(tile.Cards.Where(c => c.ID == card.ID).FirstOrDefault(), this));
                }

                foreach (var oldCard in Tile.Cards)
                {
                    Tile.Cards.Remove(Tile.Cards.First(s => s.ID == oldCard.ID));
                    TileImage.OverImageContent.RemoveRange(TileImage.OverImageContent.Where(c => c is CardContainer card && card.Card.ID == oldCard.ID));
                }

            }
            Tile.Tokens.Clear();
            TileImage.OverImageContent.RemoveRange(TileImage.OverImageContent.Where(t => t is TokenContainer));
            foreach (var token in tile.Tokens)
            {
                Token newToken;
                Tile.Tokens.Add(newToken = new Token
                {
                    ID = token.TypeID,
                    Amount = token.Count
                });
                TileImage.OverImageContent.Add(new TokenContainer(newToken));
            }
        }
    }
}
