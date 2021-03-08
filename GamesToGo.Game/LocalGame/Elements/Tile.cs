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

        public List<Card> Cards { get; } = new List<Card>();

        public List<Token> Tokens { get; } = new List<Token>();

        public ElementOrientation Orientation { get; set; }

        public Vector2 Position { get; set; }
    }
}
