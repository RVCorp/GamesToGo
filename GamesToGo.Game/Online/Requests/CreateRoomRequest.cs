using System.Net.Http;
using GamesToGo.Game.Online.Models.RequestModel;
using osu.Framework.IO.Network;

namespace GamesToGo.Game.Online.Requests
{
    public class CreateRoomRequest : APIRequest<OnlineRoom>
    {
        private int id;
        public CreateRoomRequest(int id)
        {
            this.id = id;
        }

        protected override WebRequest CreateWebRequest()
        {
            var req = base.CreateWebRequest();
            req.Method = HttpMethod.Post;
            req.AddParameter("gameID", @$"{id}");
            return req;
        }
        protected override string Target => "Room/CreateRoom";
    }
}




