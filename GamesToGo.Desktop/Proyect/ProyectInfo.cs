using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Database.Models
{
    public class ProyectInfo
    {
        public int LocalProyectID { get; set; }
        public int CreatorID { get; set; }
        public string Name { get; set; }
        public int MinNumberPlayers { get; set; }
        public int MaxNumberPlayers { get; set; }
        public int NumberCards { get; set; }
        public int NumberTokens { get; set; }
        public int NumberBoxes { get; set; }
        public int OnlineProyecrID { get; set; }
        public int ModerationStatus { get; set; }
        public int ComunityStatus { get; set; }
        public ICollection<FileRelation> Relations { get; set; }
    }
}
