using System.Collections.Generic;
using GamesToGo.Common.Online.RequestModel;
using GamesToGo.Common.Online.Requests;

namespace GamesToGo.Game.Online.Requests
{
    public class GetAllPublishedGamesRequest : APIRequest<List<OnlineGame>>
    {
        protected override string Target => "Games/AllPublishedGames";
    }
}
