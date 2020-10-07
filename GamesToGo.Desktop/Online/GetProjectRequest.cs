namespace GamesToGo.Desktop.Online
{
    // ReSharper disable once UnusedType.Global
    public class GetProjectRequest : APIRequest<OnlineProject>
    {
        private readonly int gameID;
        public GetProjectRequest(int id)
        {
            gameID = id;
        }
        protected override string Target => $"games/{gameID}";
    }
}
