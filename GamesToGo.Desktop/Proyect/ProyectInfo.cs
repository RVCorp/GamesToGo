using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Desktop.Database.Models;

namespace GamesToGo.Desktop.Proyect
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
        public File File { get; set; }
        public int FileID { get; set; }
        public ICollection<FileRelation> Relations { get; set; }
    }
}
