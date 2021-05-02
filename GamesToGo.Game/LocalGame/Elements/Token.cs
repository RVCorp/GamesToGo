using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Game.LocalGame.Elements
{
    public class Token : GameElement
    { 
        public int Amount { get; set; }
        public override ElementType Type => ElementType.Token;
    }
}
