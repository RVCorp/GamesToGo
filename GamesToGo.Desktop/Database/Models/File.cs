using System;
using System.Collections.Generic;
using System.Text;

namespace GamesToGo.Desktop.Database.Models
{
    public class File
    {
        public int FileID { get; set; }
        public string OriginalName { get; set; }
        public string Type { get; set; }
        public string NewName { get; set; }
    }
}
