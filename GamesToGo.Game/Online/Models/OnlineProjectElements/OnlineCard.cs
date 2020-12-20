using System.Collections.Generic;

namespace GamesToGo.Game.Online.Models.OnlineProjectElements
{
    public class OnlineCard
    {
        public int ID { get; set; }

        public int TypeID { get; set; }

        public int Orientation { get; set; }

        public bool FrontVisible { get; set; }

        public List<OnlineToken> Tokens { get; set; }
    }
}
