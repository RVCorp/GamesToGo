using System.Collections.Generic;

namespace GamesToGo.Editor.Online
{
    public class GetAllPublishedProjectsRequest : APIRequest<List<OnlineProject>>
    {
        protected override string Target => "games/UserPublishedGames";
    }
}
