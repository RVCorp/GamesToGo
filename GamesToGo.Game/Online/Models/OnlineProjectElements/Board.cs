using System.Collections.Generic;

namespace GamesToGo.Game.Online.Models.OnlineProjectElements
{
    public class Board
    {
        public int TypeID { get; set; }
        public int ID { get; set; }
        public List<Tile> Tiles { get; set; }
    }
}
