using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Online
{
    class GetAllPublishedProjectsRequest : APIRequest<List<OnlineProject>>
    {
        protected override string Target => $"games/AllPublishedGames";
    }
}
