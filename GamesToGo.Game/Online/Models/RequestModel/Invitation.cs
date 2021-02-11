using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Common.Online.RequestModel;

namespace GamesToGo.Game.Online.Models.RequestModel
{
    public class Invitation
    {
        public int ID { get; set; }
        public DateTime TimeSent { get; set; }
        public User Sender { get; set; }
        public User Receiver { get; set; }
        public RoomPreview Room { get; set; }
    }
}
