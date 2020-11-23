using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using osu.Framework.IO.Network;

namespace GamesToGo.App.Online
{
    public class JoinRoomRequest : APIRequest<OnlineRoom>
    {
        private int id;
        public JoinRoomRequest(int id)
        {
            this.id = id;
        }

        protected override WebRequest CreateWebRequest()
        {
            var req = base.CreateWebRequest();
            req.Method = HttpMethod.Post;
            req.AddParameter("id", @$"{id}");
            return req;
        }
        protected override string Target => $"Room/JoinRoom";
    }
}
