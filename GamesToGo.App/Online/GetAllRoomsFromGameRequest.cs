using System.Collections.Generic;

namespace GamesToGo.App.Online
{
    public class GetAllRoomsFromGameRequest : APIRequest<List<RoomPreview>>
    {
        private int id;
        public GetAllRoomsFromGameRequest(int id)
        {
            this.id = id;
        }

        protected override string Target => $"Rooms/AllRoomsFrom/{id}";
    }
}
