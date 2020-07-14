using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Desktop.Project.Elements;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using GamesToGo.Desktop.Database.Models;

namespace GamesToGo.Desktop.Project
{
    public class WorkingProject
    {
        private readonly TextureStore textures;
        private readonly Context database;
        private readonly Storage store;

        public ProjectInfo DatabaseObject { get; }

        private int latestElementID = 1;

        private readonly BindableList<ProjectElement> projectElements = new BindableList<ProjectElement>();

        public IEnumerable<Card> ProjectCards => projectElements.Where(e => e is Card).Select(e => (Card)e);

        public IEnumerable<Token> ProjectTokens => projectElements.Where(e => e is Token).Select(e => (Token)e);

        public IEnumerable<Board> ProjectBoards => projectElements.Where(e => e is Board).Select(e => (Board)e);

        public IEnumerable<Tile> ProjectTiles => projectElements.Where(e => e is Tile).Select(e => (Tile)e);

        public IBindableList<ProjectElement> ProjectElements => projectElements;

        public List<Image> Images = new List<Image>();

        public 

        protected WorkingProject(ProjectInfo project, Storage store, TextureStore textures, Context database)
        {
            DatabaseObject = project;
            this.store = store;
            this.textures = textures;
            this.database = database;
        }

        public static WorkingProject Parse(ProjectInfo project, Storage store, TextureStore textures, Context database)
        {
            WorkingProject ret = new WorkingProject(project, store, textures, database);

            if (project.File != null)
            {
                if (GamesToGoEditor.HashBytes(System.IO.File.ReadAllBytes(store.GetFullPath($"files/{project.File.NewName}"))) != project.File.NewName)
                    return null;
                ret.parse(System.IO.File.ReadAllLines(store.GetFullPath($"files/{project.File.NewName}")));
            }

            ret.ProjectElements.ItemsAdded += _ => ret.updateDatabaseObjectInfo();
            ret.ProjectElements.ItemsRemoved += _ => ret.updateDatabaseObjectInfo();

            return ret;
        }

        private void updateDatabaseObjectInfo()
        {
            DatabaseObject.NumberBoxes = ProjectTiles.Count();
            DatabaseObject.NumberCards = ProjectCards.Count();
            DatabaseObject.NumberTokens = ProjectTokens.Count();
            DatabaseObject.NumberBoards = ProjectBoards.Count();
        }

        public void AddElement(ProjectElement element)
        {
            element.ID = latestElementID++;
            projectElements.Add(element);
        }

        public void AddImage(File image)
        {
            Images.Add(new Image(textures, image));
        }

        /// <summary>
        /// Solo llamar cuando se quiera guardar
        /// </summary>
        /// <returns></returns>
        public string SaveableString()
        {
            DatabaseObject.ComunityStatus = CommunityStatus.Saved;

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("[Info]");
            builder.AppendLine("CreatorID=-1");
            builder.AppendLine($"Name={DatabaseObject.Name}");
            builder.AppendLine($"MinNumberPlayers={DatabaseObject.MinNumberPlayers}");
            builder.AppendLine($"MaxNumberPlayers={DatabaseObject.MaxNumberPlayers}");
            builder.AppendLine($"ChatRecommendation={}")
            builder.AppendLine($"Files={Images.Count}");
            foreach (var img in Images)
            {
                builder.AppendLine($"{img.DatabaseObject.NewName}");
            }
            builder.AppendLine($"LastEdited={(DatabaseObject.LastEdited = DateTime.Now).ToUniversalTime():yyyyMMddHHmmssfff}");
            builder.AppendLine();

            builder.AppendLine("[Objects]");
            foreach (ProjectElement elem in ProjectElements)
            {
                builder.AppendLine($"{elem.ToSaveable()}");
                builder.AppendLine();
            }

            return builder.ToString();
        }

        private bool parse(string[] lines)
        {
            bool isParsingObjects = false;

            ProjectElement parsingElement = null;

            foreach (var line in lines)
            {
                if (line.StartsWith('['))
                {
                    switch (line.Trim(new char[] { '[', ']' }))
                    {
                        case "Info":
                            isParsingObjects = false;
                            break;
                        case "Objects":
                            isParsingObjects = true;
                            break;
                    }
                    continue;
                }

                if (isParsingObjects)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        if (parsingElement != null)
                        {
                            AddElement(parsingElement);
                            parsingElement = null;
                        }
                        continue;
                    }

                    if (parsingElement == null)
                    {
                        var idents = line.Split('|', 3);
                        if (idents.Length != 3)
                            return false;
                        switch (int.Parse(idents[0]))
                        {
                            case 0:
                                parsingElement = new Token();
                                break;
                            case 1:
                                parsingElement = new Card();
                                break;
                            case 2:
                                parsingElement = new Tile();
                                break;
                            case 3:
                                parsingElement = new Board();
                                break;
                        }
                        parsingElement.ID = int.Parse(idents[1]);
                        parsingElement.Name.Value = idents[2];
                        continue;
                    }
                }
            }

            return true;
        }
    }
}
