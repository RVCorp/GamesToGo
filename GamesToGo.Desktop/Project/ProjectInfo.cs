using System;
using System.Collections.Generic;
using GamesToGo.Desktop.Database.Models;

namespace GamesToGo.Desktop.Project
{
    public class ProjectInfo
    {
        public int LocalProjectID { get; set; }

        public int CreatorID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int MinNumberPlayers { get; set; }

        public int MaxNumberPlayers { get; set; }

        public int NumberCards { get; set; }

        public int NumberTokens { get; set; }

        public int NumberBoxes { get; set; }

        public int NumberBoards { get; set; }


        public int OnlineProjectID { get; set; }

        public int ModerationStatus { get; set; }

        public CommunityStatus ComunityStatus { get; set; }

        public DateTime LastEdited { get; set; }

        public FileRelation ImageRelation { get; set; }

        public int? ImageRelationID { get; set; }

        public File File { get; set; }

        public int FileID { get; set; }

        public ICollection<FileRelation> Relations { get; set; } = new List<FileRelation>();
    }
}
