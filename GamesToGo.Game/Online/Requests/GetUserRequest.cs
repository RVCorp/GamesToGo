using GamesToGo.Game.Online.Models.RequestModel;

namespace GamesToGo.Game.Online.Requests
{
    public class GetUserRequest : APIRequest<User>
    {
        private readonly int userID;

        public GetUserRequest(int id)
        {
            userID = id;
        }

        protected override string Target => $"users/{userID}";
    }
}
