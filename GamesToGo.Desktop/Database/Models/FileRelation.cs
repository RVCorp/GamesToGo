using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Database.Models
{
    public class FileRelation
    {
        public int ProyectID { get; set; }
        public ProyectInfo Proyect { get; set; }
        public int FileID { get; set; }
        public File File { get; set; }
    }
}

