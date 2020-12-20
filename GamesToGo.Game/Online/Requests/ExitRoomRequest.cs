using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using osu.Framework.IO.Network;

namespace GamesToGo.Game.Online.Requests
{
    public class ExitRoomRequest : APIRequest
    {
        protected override WebRequest CreateWebRequest()
        {
            var req = base.CreateWebRequest();
            req.Method = HttpMethod.Post;
            return req;
        }
        protected override string Target => "Room/LeaveRoom";
    }
}
