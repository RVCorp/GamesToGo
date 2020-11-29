using System.Collections.Generic;

namespace GamesToGo.Game.Online
{
    public class Card
    {
        public int ID { get; set; }

        public int TypeID { get; set; }

        public int Orientation { get; set; }

        public bool FrontVisible { get; set; }

        public List<Token> Tokens { get; set; }
    }
}
