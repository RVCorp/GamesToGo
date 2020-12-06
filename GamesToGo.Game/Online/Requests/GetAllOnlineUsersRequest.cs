using System.Collections.Generic;
using GamesToGo.Game.Online.Models.RequestModel;

namespace GamesToGo.Game.Online.Requests
{
    public class GetAllOnlineUsersRequest : APIRequest<List<User>>
    {
        protected override string Target => "Login/OnlineUsers";
    }
}
