﻿using GamesToGo.Common.Online.RequestModel;

namespace GamesToGo.Common.Online.Requests
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
