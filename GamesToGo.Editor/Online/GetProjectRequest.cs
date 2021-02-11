using GamesToGo.Common.Online.RequestModel;
using GamesToGo.Common.Online.Requests;

namespace GamesToGo.Editor.Online
{
    // ReSharper disable once UnusedType.Global
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
