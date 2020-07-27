using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Online
{
    public class GetUserRequest : APIRequest<User>
    {
        private int userID;

        public GetUserRequest(int id)
        {
            userID = id;
        }

        protected override string Target => $"users/{userID}";
    }
}
