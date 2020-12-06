using System.Collections.Generic;
using GamesToGo.Game.Online.Models.RequestModel;

namespace GamesToGo.Game.Online.Requests
{
    public class GetUserStatisticsRequest : APIRequest<List<Statistic>>
    {
        protected override string Target => "Users/Statistics";
    }
}
