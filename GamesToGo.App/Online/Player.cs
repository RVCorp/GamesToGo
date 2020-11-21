using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
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
