using System.Collections.Generic;

namespace GamesToGo.Desktop.Online
{
    public class GetAllProjectsRequest : APIRequest<List<OnlineProject>>
    {
        protected override string Target => "games/AllGames";
    }
}
