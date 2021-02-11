using System.Threading.Tasks;
using GamesToGo.Common.Online;
using GamesToGo.Common.Overlays;
using GamesToGo.Game.Online.Models.RequestModel;
using GamesToGo.Game.Online.Requests;
using GamesToGo.Game.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Screens;

namespace GamesToGo.Game.Online
{
    public class RoomUpdater : PollingComponent
    {
        public override bool IsPresent => true;

        [Resolved]
        private Bindable<OnlineRoom> room { get; set; }

        [Resolved]
        private APIController api { get; set; }

        [Resolved]
        private SplashInfoOverlay infoOverlay { get; set; }

        [Resolved]
        private MainMenuScreen mainMenu { get; set; }

        private GetRoomStateRequest roomStateRequest;

        protected override Task Poll()
        {
            var completionSource = new TaskCompletionSource<bool>();

            roomStateRequest?.Cancel();
            roomStateRequest = new GetRoomStateRequest();


            roomStateRequest.Success += newRoom =>
            {
                if (newRoom.Equals(room.Value))
                    room.Value.TimeElapsed = newRoom.TimeElapsed;
                else
                    room.Value = newRoom;
                completionSource.SetResult(true);
            };

            roomStateRequest.Failure += _ =>
            {
                infoOverlay.Show(@"La sala se ha cerrado", Colour4.DarkGreen);
                mainMenu.MakeCurrent();
                completionSource.SetResult(false);
            };

            api.Queue(roomStateRequest);

            return completionSource.Task;
        }
    }
}
