using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Online
{
    public class GetAllProjectsRequest : APIRequest<List<Project>>
    {
        protected override string Target => $"games/AllGames";
    }
}
