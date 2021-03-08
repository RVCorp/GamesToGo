using System.Collections.Generic;
using System.Linq;
using GamesToGo.Common.Online.RequestModel;
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

        public double TimeElapsed { get; set; }

        public Player PlayerWithID(int id) => Players.SingleOrDefault(p => p?.BackingUser.ID == id);

        public override bool Equals(object obj)
        {
            if (!(obj is OnlineRoom other))
                return false;

            bool playersEqual = Players.Length == other.Players.Length;

            for (int i = 0; i < Players.Length && playersEqual; i++)
                playersEqual &= Players[i] == null && other.Players[i] == null ||
                                Players[i] != null && Players[i].Equals(other.Players[i]);

            if (!playersEqual)
                return false;

            bool boardsEqual = Boards.Count == other.Boards.Count;

            for (int i = 0; i < Boards.Count && boardsEqual; i++)
                boardsEqual &= Boards[i].Equals(other.Boards[i]);

            if (!boardsEqual)
                return false;

            return Owner.Equals(other.Owner) &&
                   ID == other.ID &&
                   HasStarted == other.HasStarted;
        }
    }
}
