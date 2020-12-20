using System.Collections.Generic;
using GamesToGo.Game.Online.Models.OnlineProjectElements;

namespace GamesToGo.Game.Online.Models.RequestModel
{
    public class OnlineRoom
    {
        public Player Owner { get; set; }
        public int ID { get; set; }
        public OnlineGame Game { get; set; }

        public Player[] Players { get; set; }

        public List<OnlineBoard> Boards { get; set; }

        public bool HasStarted { get; set; }

        public double TimeElapsed { get; set; }
    }
}
