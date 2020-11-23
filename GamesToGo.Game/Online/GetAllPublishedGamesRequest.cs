using System.Collections.Generic;

namespace GamesToGo.Game.Online
{
    public class GetAllPublishedGamesRequest : APIRequest<List<OnlineGame>>
    {
        protected override string Target => "Games/AllPublishedGames";
    }
}
