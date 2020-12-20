using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace GamesToGo.Game.LocalGame.Elements
{
    public class Tile : GameElement
    {
        public override ElementType Type => ElementType.Tile;

        public Vector2 Size { get; set; }

        public ElementOrientation Orientation { get; set; }

        public Vector2 Position { get; set; }
    }
}
