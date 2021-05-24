using System.Collections.Generic;
using GamesToGo.Common.Online.RequestModel;
using GamesToGo.Common.Online.Requests;

namespace GamesToGo.Common.Online.Requests
{
    public class GetUserStatisticsRequest : APIRequest<List<Statistic>>
    {
        protected override string Target => "Users/Statistics";
    }
}
