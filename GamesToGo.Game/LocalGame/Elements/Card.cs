using System;
using System.Collections.Generic;
using System.Text;
using osuTK;

namespace GamesToGo.Game.LocalGame.Elements
{
    public class Card : GameElement
    {
        public override ElementType Type => ElementType.Card;

        public Vector2 Size { get; set; }
    }
}
