using System.Net.Http;
using GamesToGo.Common.Online.Requests;
using GamesToGo.Game.Online.Models.RequestModel;
using osu.Framework.IO.Network;

namespace GamesToGo.Game.Online.Requests
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
        protected override string Target => "Room/JoinRoom";
    }
}
