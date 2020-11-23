using System;
using System.Collections.Generic;
using GamesToGo.Editor.Database.Models;
using GamesToGo.Editor.Project;
using Newtonsoft.Json;
// ReSharper disable All

namespace GamesToGo.Editor.Project
{
    public class ProjectInfo
    {
        public int LocalProjectID { get; set; }

        public int CreatorID { get; set; }

        public string Name { get; set; } = @"Nuevo Proyecto";

        public string Description { get; set; } = @"¡Decribe tu proyecto en una frase!";

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

        [JsonIgnore]
        public FileRelation ImageRelation { get; set; }

        public int? ImageRelationID { get; set; }

        [JsonIgnore]
        public File File { get; set; }

        [JsonIgnore]
        public int FileID { get; set; }

        [JsonIgnore]
        public ICollection<FileRelation> Relations { get; set; } = new List<FileRelation>();
    }
}
