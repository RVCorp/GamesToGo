using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesToGo.Common.Online;
using GamesToGo.Common.Overlays;
using GamesToGo.Game.Online.Models.RequestModel;
using GamesToGo.Game.Online.Requests;
using osu.Framework.Allocation;
using osu.Framework.Bindables;

namespace GamesToGo.Game.Online
{
    public class InvitationsUpdater : PollingComponent
    {
        private GetAllInvitationsRequest invitationsRequest;
        public override bool IsPresent => true;

        [Resolved]
        private BindableList<Invitation> invitations { get; set; }

        [Resolved]
        private APIController api { get; set; }

        [Resolved]
        private SplashInfoOverlay infoOverlay { get; set; }

        public InvitationsUpdater() : base(5000)
        {

        }

        protected override Task Poll()
        {
            if (api.LocalUser.Value == null)
                return base.Poll();

            var completionSource = new TaskCompletionSource<bool>();

            invitationsRequest?.Cancel();
            invitationsRequest = new GetAllInvitationsRequest();

            invitationsRequest.Success += onlineInvitations =>
            {
                List<Invitation> localInvitations = invitations.ToList();

                for (int i = 0; i < localInvitations.Count; i++)
                {
                    // ReSharper disable AccessToModifiedClosure
                    if (onlineInvitations.All(t => t.ID != localInvitations[i].ID))
                        continue;

                    onlineInvitations.Remove(onlineInvitations.First(p => p.ID == localInvitations[i].ID));
                    localInvitations.Remove(localInvitations[i]);
                    i--;

                    // ReSharper restore AccessToModifiedClosure
                }

                if (localInvitations.Any() || onlineInvitations.Any())
                {
                    invitations.AddRange(onlineInvitations.Select(i => new Invitation
                    {
                        ID = i.ID,
                        TimeSent = i.TimeSent,
                        Sender = i.Sender,
                        Receiver = i.Receiver,
                        Room = i.Room,
                    }));

                    foreach (var oldInvite in localInvitations)
                    {
                        invitations.Remove(invitations.First(s => s.ID == oldInvite.ID));
                    }
                }

                completionSource.SetResult(true);
            };

            invitationsRequest.Failure += _ => completionSource.SetResult(false);

            api.Queue(invitationsRequest);

            return completionSource.Task;
        }
    }
}
