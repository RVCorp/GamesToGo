using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace GamesToGo.Game.LocalGame.Elements
{
    public class Board : GameElement
    {
        public override ElementType Type => ElementType.Board;

        public Vector2 Size { get; set; }

        public List<Tile> Tiles { get; } = new List<Tile>();

        public Queue<int> PendingElements { get; } = new Queue<int>();

    }
}
