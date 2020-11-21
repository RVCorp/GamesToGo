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

namespace GamesToGo.App.Online
{
    public class OnlineRoom
    {
        public User Owner { get; set; }
        public int ID { get; set; }
        public OnlineGame Game { get; set; }

        public Player[] Players { get; set; }

        public List<Board> Boards { get; set; }

        public bool HasStarted { get; set; }

        public double TimeElapsed { get; set; }
    }
}
