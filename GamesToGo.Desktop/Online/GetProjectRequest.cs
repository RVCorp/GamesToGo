using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Online
{
    public class GetProjectRequest : APIRequest<Project>
    {
        private int gameID;
        public GetProjectRequest(int id)
        {
            gameID = id;
        }
        protected override string Target => $"games/{gameID}";
    }
}
