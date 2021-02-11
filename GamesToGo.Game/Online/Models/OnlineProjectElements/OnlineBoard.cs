using System.Collections.Generic;

namespace GamesToGo.Game.Online.Models.OnlineProjectElements
{
    public class OnlineBoard
    {
        public int TypeID { get; set; }
        public List<OnlineTile> Tiles { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is OnlineBoard other))
                return false;
            bool tilesEqual = Tiles.Count == other.Tiles.Count;

            for (int i = 0; i < Tiles.Count && tilesEqual; i++)
                tilesEqual &= Tiles[i].Equals(other.Tiles[i]);

            return tilesEqual &&
                TypeID == other.TypeID;
        }
    }
}
