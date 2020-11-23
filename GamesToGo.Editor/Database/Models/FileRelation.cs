using GamesToGo.Editor.Project;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace GamesToGo.Editor.Database.Models
{
    public class FileRelation
    {
        public int RelationID { get; set; }

        public int ProjectID { get; set; }

        public ProjectInfo Project { get; set; }

        public int FileID { get; set; }

        public File File { get; set; }
    }
}

