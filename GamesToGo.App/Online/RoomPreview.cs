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
    public class RoomPreview
    {
        public int Id { get; set; }
        public User Owner { get; set; }
        public int PlayersInRoom { get; set; }
    }
}
