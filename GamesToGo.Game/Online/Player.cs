using System.Collections.Generic;
using Newtonsoft.Json;

namespace GamesToGo.Game.Online
{
    public class Player
    {
        public int RoomPosition { get; set; }

        [JsonProperty(@"User")]
        public User BackingUser { get; set; }

        public bool Ready { get; set; }

        public Tile Tile { get; set; }

        public List<Card> PlayerHand { get; set; }
    }
}
