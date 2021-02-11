using System.Collections.Generic;
using GamesToGo.Common.Online.RequestModel;
using GamesToGo.Common.Online.Requests;
using GamesToGo.Game.Online.Models.RequestModel;

namespace GamesToGo.Game.Online.Requests
{
    public class GetAllOnlineUsersRequest : APIRequest<List<User>>
    {
        protected override string Target => "Login/OnlineUsers";
    }
}
