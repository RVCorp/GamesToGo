using GamesToGo.Desktop.Project;

namespace GamesToGo.Desktop.Database.Models
{
    public class FileRelation
    {
        public int ProjectID { get; set; }

        public ProjectInfo Project { get; set; }

        public int FileID { get; set; }

        public File File { get; set; }
    }
}

