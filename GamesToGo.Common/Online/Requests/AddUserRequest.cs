using System.Net.Http;
using GamesToGo.Common.Online.RequestModel;
using Newtonsoft.Json;
using osu.Framework.IO.Network;

namespace GamesToGo.Common.Online.Requests
{
    internal class AddUserRequest : APIRequest<User>
    {
        private UserLogin user;

        public AddUserRequest(UserLogin newUser)
        {
            user = newUser;
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
