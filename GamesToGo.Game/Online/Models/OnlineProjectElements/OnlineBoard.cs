using System.Collections.Generic;

namespace GamesToGo.Game.Online.Models.OnlineProjectElements
{
    public class OnlineBoard
    {
        public int TypeID { get; set; }
        public List<OnlineTile> Tiles { get; set; }
    }
}
