using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Game.LocalGame.Elements
{
    public class Token : GameElement
    {
        public override ElementType Type => ElementType.Token;

        public ElementPrivacy Privacy { get; set; }
    }
}
