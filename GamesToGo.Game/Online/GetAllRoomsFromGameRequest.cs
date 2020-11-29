using System.Collections.Generic;

namespace GamesToGo.Game.Online
{
    public class GetAllRoomsFromGameRequest : APIRequest<List<RoomPreview>>
    {
        private int id;
        public GetAllRoomsFromGameRequest(int id)
        {
            this.id = id;
        }

        protected override string Target => $"Room/AllRoomsFor/{id}";
    }
}
