using System;
using System.Collections.Generic;
using System.Text;
using osuTK;

namespace GamesToGo.Game.LocalGame.Elements
{
    public class Card : GameElement
    {
        public int TypeID { get; set; }
        public override ElementType Type => ElementType.Card;

        public Vector2 Size { get; set; }

        public bool FrontVisible { get; set; }

        public List<Token> Tokens { get; set; } = new List<Token>();

        public ElementPrivacy Privacy { get; set; }

        public ElementOrientation Orientation { get; set; }
    }
}
