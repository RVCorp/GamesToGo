using System.Collections.Generic;

namespace GamesToGo.Game.Online
{
    public class GetAllProjectsRequest : APIRequest<List<OnlineGame>>
    {
        protected override string Target => "games/AllGames";
    }
}
