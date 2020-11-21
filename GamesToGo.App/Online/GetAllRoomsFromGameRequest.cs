using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

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
