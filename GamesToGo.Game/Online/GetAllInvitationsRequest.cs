using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Bindables;

namespace GamesToGo.Game.Online
{
    public class GetAllInvitationsRequest : APIRequest<List<Invitation>>
    {
        protected override string Target => "Users/Invitations";
    }
}
