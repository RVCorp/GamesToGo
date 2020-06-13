using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Desktop.Project;

namespace GamesToGo.Desktop.Database.Models
{
    public class File
    {
        public int FileID { get; set; }

        public string OriginalName { get; set; }

        public string Type { get; set; }

        public string NewName { get; set; }

        public ICollection<FileRelation> Relations { get; set; }

        public ProjectInfo Project { get; set; }
    }
}
