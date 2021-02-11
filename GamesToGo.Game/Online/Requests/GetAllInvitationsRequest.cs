using System.Collections.Generic;
using GamesToGo.Common.Online.Requests;
using GamesToGo.Game.Online.Models.RequestModel;

namespace GamesToGo.Game.Online.Requests
{
    public class GetAllInvitationsRequest : APIRequest<List<Invitation>>
    {
        protected override string Target => "Users/Invitations";
    }
}
