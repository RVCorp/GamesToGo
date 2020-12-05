using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Game.Online
{
    public class GetAllOnlineUsersRequest : APIRequest<List<User>>
    {
        protected override string Target => "Login/OnlineUsers";
    }
}
