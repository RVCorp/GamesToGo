﻿using GamesToGo.Common.Online.RequestModel;
using GamesToGo.Common.Online.Requests;
using GamesToGo.Game.Online.Models.RequestModel;

namespace GamesToGo.Game.Online.Requests
{
    public class GetProjectRequest : APIRequest<OnlineGame>
    {
        private readonly int gameID;
        public GetProjectRequest(int id)
        {
            gameID = id;
        }
        protected override string Target => $"games/{gameID}";
    }
}
