using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using osu.Framework.IO.Network;

namespace GamesToGo.Game.Online
{
    public class ReadyRequest : APIRequest
    {
        protected override WebRequest CreateWebRequest()
        {
            var req = base.CreateWebRequest();
            req.Method = HttpMethod.Post;
            return req;
        }
        protected override string Target => "Room/Ready";
    }
}
