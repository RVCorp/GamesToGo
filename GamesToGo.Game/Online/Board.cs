using System.Collections.Generic;

namespace GamesToGo.Game.Online
{
    public class Board
    {
        public int TypeID { get; set; }
        public int ID { get; set; }
        public List<Tile> Tiles { get; set; }
    }
}
