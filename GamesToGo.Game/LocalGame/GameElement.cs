using System;
using System.Collections.Generic;
using System.Linq;
using GamesToGo.Game.LocalGame.Elements;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace GamesToGo.Game.LocalGame
{
    public abstract class GameElement
    {
        public int TypeID { get; set; }

        public abstract ElementType Type { get; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<Texture> Images { get; } = new List<Texture>();
    }
}
