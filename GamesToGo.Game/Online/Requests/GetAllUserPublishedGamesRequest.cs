using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Game.Online.Models.RequestModel;

namespace GamesToGo.Game.Online.Requests
{
    public class GetAllUserPublishedGamesRequest : APIRequest<List<OnlineGame>>
    {
        protected override string Target => "Games/UserPublishedGames";
    }
}
