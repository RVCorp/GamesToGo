using System.Collections.Generic;

namespace GamesToGo.App.Online
{
    public class GetAllPublishedGamesRequest : APIRequest<List<OnlineGame>>
    {
        protected override string Target => "Games/AllPublishedGames";
    }
}
