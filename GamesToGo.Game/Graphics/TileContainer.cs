using System.Collections.Generic;
using System.Linq;
using GamesToGo.Game.LocalGame.Elements;
using GamesToGo.Game.Online.Models.OnlineProjectElements;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace GamesToGo.Game.Graphics
{
    public class TileContainer : Container
    {
        private Tile tile;

        public List<Card> Cards = new List<Card>();

        public TileContainer(Tile tile)
        {
            this.tile = tile;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            FillAspectRatio = tile.Size.X / tile.Size.Y;            
            FillMode = FillMode.Fit;
            Masking = true;
            BorderColour = Colour4.Black;
            BorderThickness = .5f;
            Position = tile.Position;
            Children = new Drawable[]
            {
                new Sprite
                {
                    RelativeSizeAxes = Axes.Both,
                    Texture = tile.Images.First()
                }
            };
        }
    }
}
