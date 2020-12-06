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

        public Tile Hand { get; set; }

        public PlayerStatus Status { get; set; }
    }
}
