using System.Collections.Generic;
using GamesToGo.Game.Online.Models.RequestModel;

namespace GamesToGo.Game.Online.Requests
{
    public class GetAllPublishedProjectsRequest : APIRequest<List<OnlineGame>>
    {
        protected override string Target => "games/AllPublishedGames";
    }
}
