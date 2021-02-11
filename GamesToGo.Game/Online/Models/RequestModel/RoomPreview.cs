using GamesToGo.Common.Online.RequestModel;

namespace GamesToGo.Game.Online.Models.RequestModel
{
    public class RoomPreview
    {
        public int Id { get; set; }
        public User Owner { get; set; }
        public int CurrentPlayers { get; set; }

        public OnlineGame Game { get; set; }
    }
}
