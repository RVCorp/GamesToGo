using System.Collections.Generic;

namespace GamesToGo.App.Online
{
    public class Board
    {
        public int TypeID { get; set; }
        public int ID { get; set; }
        public List<Tile> Tiles { get; set; }
    }
}
