using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Online
{
    public class GetAllProjectsRequest : APIRequest<List<OnlineProject>>
    {
        protected override string Target => $"games/AllGames";
    }
}
