using System.Collections.Generic;

namespace GamesToGo.Game.Online
{
    public class GetAllPublishedProjectsRequest : APIRequest<List<OnlineGame>>
    {
        protected override string Target => "games/AllPublishedGames";
    }
}
