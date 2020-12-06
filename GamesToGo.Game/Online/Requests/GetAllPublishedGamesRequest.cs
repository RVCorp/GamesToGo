using System.Collections.Generic;
using GamesToGo.Game.Online.Models.RequestModel;

namespace GamesToGo.Game.Online.Requests
{
    public class GetAllPublishedGamesRequest : APIRequest<List<OnlineGame>>
    {
        protected override string Target => "Games/AllPublishedGames";
    }
}
