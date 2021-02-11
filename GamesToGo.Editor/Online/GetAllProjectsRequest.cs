using System.Collections.Generic;
using GamesToGo.Common.Online.RequestModel;
using GamesToGo.Common.Online.Requests;

namespace GamesToGo.Editor.Online
{
    public class GetAllProjectsRequest : APIRequest<List<OnlineGame>>
    {
        protected override string Target => "games/AllGames";
    }
}
