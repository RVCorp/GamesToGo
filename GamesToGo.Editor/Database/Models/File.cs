using System.Collections.Generic;
using GamesToGo.Editor.Project;

// ReSharper disable CollectionNeverUpdated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GamesToGo.Editor.Database.Models
{
    public class File
    {
        public int FileID { get; set; }

        public string OriginalName { get; set; } = "";

        public string Type { get; set; }

        public string NewName { get; set; }

        public ICollection<FileRelation> Relations { get; set; }

        public ProjectInfo Project { get; set; }
    }
}
