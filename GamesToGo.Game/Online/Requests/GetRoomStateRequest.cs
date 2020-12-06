using GamesToGo.Game.Online.Models.RequestModel;

namespace GamesToGo.Game.Online.Requests
{
    public class GetRoomStateRequest : APIRequest<OnlineRoom>
    {
        protected override string Target => "Room/RoomState";
    }
}
