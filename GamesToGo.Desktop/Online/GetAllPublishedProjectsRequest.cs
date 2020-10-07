using System.Collections.Generic;

namespace GamesToGo.Desktop.Online
{
    public class GetAllPublishedProjectsRequest : APIRequest<List<OnlineProject>>
    {
        protected override string Target => "games/AllPublishedGames";
    }
}
