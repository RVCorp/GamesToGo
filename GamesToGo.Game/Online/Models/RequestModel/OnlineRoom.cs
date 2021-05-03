using System.Collections.Generic;
using System.Linq;
using GamesToGo.Common.Online.RequestModel;
using GamesToGo.Game.LocalGame.Arguments;
using GamesToGo.Game.LocalGame.Elements;
using GamesToGo.Game.Online.Models.OnlineProjectElements;

namespace GamesToGo.Game.Online.Models.RequestModel
{
    public class OnlineRoom
    {
        public Player Owner { get; set; }
        public int ID { get; set; }
        public OnlineGame Game { get; set; }

        public Player[] Players { get; set; }

        public List<OnlineBoard> Boards { get; set; } = new List<OnlineBoard>();

        public bool HasStarted { get; set; }

        public bool HasEnded { get; set; }

        public double TimeElapsed { get; set; }
        public List<int> WinningPlayersIndexes { get; set; }

        public ArgumentParameter UserActionArgument { get; set; }

        public Player PlayerWithID(int id) => Players.SingleOrDefault(p => p?.BackingUser.ID == id);
    }
}
