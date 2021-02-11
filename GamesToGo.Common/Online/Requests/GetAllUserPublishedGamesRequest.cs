using System.Collections.Generic;
using GamesToGo.Common.Online.RequestModel;

namespace GamesToGo.Common.Online.Requests
{
    public class GetAllUserPublishedGamesRequest : APIRequest<List<OnlineGame>>
    {
        protected override string Target => "Games/UserPublishedGames";
    }
}
