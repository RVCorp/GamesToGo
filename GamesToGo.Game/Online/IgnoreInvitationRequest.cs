using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using osu.Framework.IO.Network;

namespace GamesToGo.Game.Online
{
    public class IgnoreInvitationRequest : APIRequest
    {
        private int id;

        public IgnoreInvitationRequest(int id)
        {
            this.id = id;
        }

        protected override WebRequest CreateWebRequest()
        {
            var req = base.CreateWebRequest();
            req.Method = HttpMethod.Post;
            req.AddParameter("invitationID", @$"{id}");
            return req;
        }
        protected override string Target => "Users/IgnoreInvitation";
    }
}
