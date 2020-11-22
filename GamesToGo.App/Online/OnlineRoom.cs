﻿using System.Collections.Generic;

namespace GamesToGo.App.Online
{
    public class OnlineRoom
    {
        public User Owner { get; set; }
        public int ID { get; set; }
        public OnlineGame Game { get; set; }

        public Player[] Players { get; set; }

        public List<Board> Boards { get; set; }

        public bool HasStarted { get; set; }

        public double TimeElapsed { get; set; }
    }
}