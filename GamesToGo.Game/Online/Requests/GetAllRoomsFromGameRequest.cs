using System.Collections.Generic;
using GamesToGo.Common.Online.Requests;
using GamesToGo.Game.Online.Models.RequestModel;

namespace GamesToGo.Game.Online.Requests
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
