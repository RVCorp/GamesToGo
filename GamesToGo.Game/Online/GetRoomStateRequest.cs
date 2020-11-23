namespace GamesToGo.Game.Online
{
    public class GetRoomStateRequest : APIRequest<OnlineRoom>
    {
        protected override string Target => "Room/RoomState";
    }
}
