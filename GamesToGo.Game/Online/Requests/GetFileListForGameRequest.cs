using System.Collections.Generic;
using GamesToGo.Common.Online.Requests;

namespace GamesToGo.Game.Online.Requests
{
    public class GetFileListForGameRequest : APIRequest<List<string>>
    {
        private readonly int gameID;

        public GetFileListForGameRequest(int gameID)
        {
            this.gameID = gameID;
        }

        protected override string Target => $"Games/GameFiles/{gameID}";
    }
}
