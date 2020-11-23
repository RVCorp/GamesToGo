using System.Collections.Generic;

namespace GamesToGo.Game.Online
{
    public class Tile
    {
         public int ID { get; set; }

         public int TypeID { get; set; }

         public  List<Token> Tokens { get; set; }

         public List<Card> Cards { get; set; }
    }
}
