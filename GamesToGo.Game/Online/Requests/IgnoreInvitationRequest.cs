using System.Net.Http;
using GamesToGo.Common.Online.Requests;
using osu.Framework.IO.Network;

namespace GamesToGo.Game.Online.Requests
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
