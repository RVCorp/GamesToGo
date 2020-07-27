using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Online
{
    public class OnlineProject
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string LastEdited { get; set; }
        public string Name { get; set; }
        public string Hash { get; set; }
        public string Description { get; set; }
        public int Minplayers { get; set; }
        public int Maxplayers { get; set; }
        public int CreatorId { get; set; }
    }
}
