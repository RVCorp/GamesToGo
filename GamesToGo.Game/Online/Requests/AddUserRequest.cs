using System.Net.Http;
using GamesToGo.Game.Online.Models.RequestModel;
using Newtonsoft.Json;
using osu.Framework.IO.Network;

namespace GamesToGo.Game.Online.Requests
{
    public class AddUserRequest : APIRequest<User>
    {
        private readonly PasswordedUser user;

        public AddUserRequest(PasswordedUser user)
        {
            this.user = user;
        }

        protected override WebRequest CreateWebRequest()
        {
            var req = base.CreateWebRequest();

            req.Method = HttpMethod.Post;
            req.AddRaw(JsonConvert.SerializeObject(user));
            req.ContentType = "text/json";
            return req;
        }

        protected override string Target => "Users";
    }
}
