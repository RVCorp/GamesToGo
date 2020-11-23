namespace GamesToGo.App.Online
{
    public class GetRoomStateRequest : APIRequest<OnlineRoom>
    {
        protected override string Target => "Room/RoomState";
    }
}
