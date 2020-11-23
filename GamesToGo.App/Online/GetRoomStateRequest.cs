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
    public class GetRoomStateRequest : APIRequest<OnlineRoom>
    {
        protected override string Target => "Room/RoomState";
    }
}
