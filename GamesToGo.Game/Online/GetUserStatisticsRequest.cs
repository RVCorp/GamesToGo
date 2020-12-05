using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Game.Online
{
    public class GetUserStatisticsRequest : APIRequest<List<Statistic>>
    {
        protected override string Target => "Users/Statistics";
    }
}
