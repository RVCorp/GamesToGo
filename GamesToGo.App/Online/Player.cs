using Newtonsoft.Json;

namespace GamesToGo.App.Online
{
    public class Player
    {
        public int RoomPosition { get; set; }

        [JsonProperty(@"User")]
        public User BackingUser { get; set; }

        public bool Ready { get; set; }

        public Tile Tile { get; set; }
    }
}
