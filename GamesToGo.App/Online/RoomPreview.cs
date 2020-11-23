namespace GamesToGo.App.Online
{
    public class RoomPreview
    {
        public int Id { get; set; }
        public User Owner { get; set; }
        public int PlayersInRoom { get; set; }

        public OnlineGame Game { get; set; }
    }
}
