using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Game.Online.Models.RequestModel
{
    public class Invitation
    {
        public int ID { get; set; }
        public DateTime TimeSent { get; set; }
        public virtual User Sender { get; set; }
        public virtual User Receiver { get; set; }
        public RoomPreview Room { get; set; }
    }
}
