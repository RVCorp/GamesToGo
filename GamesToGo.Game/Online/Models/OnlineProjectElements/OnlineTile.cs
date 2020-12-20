using System.Collections.Generic;

namespace GamesToGo.Game.Online.Models.OnlineProjectElements
{
    public class OnlineTile
    {
         public int TypeID { get; set; }

         public  List<OnlineToken> Tokens { get; set; }

         public List<OnlineCard> Cards { get; set; }
    }
}
