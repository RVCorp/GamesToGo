using GamesToGo.Common.Online.RequestModel;
using GamesToGo.Game.Online.Models.OnlineProjectElements;
using Newtonsoft.Json;

namespace GamesToGo.Game.Online.Models.RequestModel
{
    public class Player
    {
        public int RoomPosition { get; set; }

        [JsonProperty(@"User")]
        public User BackingUser { get; set; }

        public bool Ready { get; set; }

        [JsonProperty(@"Tile")]
        public OnlineTile Hand { get; } = new OnlineTile();

        public PlayerStatus Status { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Player other))
                return false;

            return RoomPosition == other.RoomPosition &&
                   BackingUser.ID == other.BackingUser.ID &&
                   Ready == other.Ready &&
                   Hand.Equals(other.Hand) &&
                   Status == other.Status;
        }
    }
}
