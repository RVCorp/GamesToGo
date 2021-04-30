using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using GamesToGo.Common.Online.Requests;
using osu.Framework.IO.Network;

namespace GamesToGo.Game.Online.Requests
{
    public class SendArgumentRequest : APIRequest
    {
        private int id;
        public SendArgumentRequest(int id)
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

        protected override string Target => "Room/Interact";
    }
}
