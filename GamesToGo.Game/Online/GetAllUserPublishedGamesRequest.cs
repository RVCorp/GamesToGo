using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Game.Online
{
    public class GetAllUserPublishedGamesRequest : APIRequest<List<OnlineGame>>
    {
        protected override string Target => "Games/UserPublishedGames";
    }
}
