using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Desktop.Project.Elements;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using GamesToGo.Desktop.Database.Models;
using GamesToGo.Desktop.Online;
using osuTK;

namespace GamesToGo.Desktop.Project
{
    public class WorkingProject
    {
        private readonly TextureStore textures;

        public ProjectInfo DatabaseObject { get; }

        private int latestElementID = 1;

        private readonly BindableList<ProjectElement> projectElements = new BindableList<ProjectElement>();

        public IEnumerable<Card> ProjectCards => projectElements.Where(e => e is Card).Select(e => (Card)e);

        public IEnumerable<Token> ProjectTokens => projectElements.Where(e => e is Token).Select(e => (Token)e);

        public IEnumerable<Board> ProjectBoards => projectElements.Where(e => e is Board).Select(e => (Board)e);

        public IEnumerable<Tile> ProjectTiles => projectElements.Where(e => e is Tile).Select(e => (Tile)e);

        public IBindableList<ProjectElement> ProjectElements => projectElements;

        public BindableList<Image> Images = new BindableList<Image>();

        public Bindable<Image> Image { get; set; } = new Bindable<Image>();

        public ChatRecommendation ChatRecommendation { get; set; }

        private int returnableSaves = 0;

        public bool FirstSave => returnableSaves > 0;

        protected WorkingProject(ref ProjectInfo project, TextureStore textures, int userID)
        {
            if (project == null)
            {
                project = new ProjectInfo { Name = "Nuevo Proyecto", CreatorID = userID };
                returnableSaves = 2;
            }

            DatabaseObject = project;
            this.textures = textures;
        }

        public static WorkingProject Parse(ProjectInfo project, Storage store, TextureStore textures, APIController api)
        {
            WorkingProject ret = new WorkingProject(ref project, textures, api.LocalUser.Value.ID);

            if (project.File != null)
            {
                try
                {
                    if (GamesToGoEditor.HashBytes(System.IO.File.ReadAllBytes(store.GetFullPath($"files/{project.File.NewName}"))) != project.File.NewName)
                        return null;
                    if (!ret.parse(System.IO.File.ReadAllLines(store.GetFullPath($"files/{project.File.NewName}"))))
                        return null;
                }
                catch
                {
                    return null;
                }

                if (project.Relations != null)
                {
                    if (project.Relations.Count != ret.Images.Count)
                        return null;
                    foreach (var image in project.Relations)
                    {
                        if (!ret.Images.Any(im => im.ImageName == image.File.NewName))
                            return null;
                    }
                }
            }

            ret.ProjectElements.ItemsAdded += _ => ret.updateDatabaseObjectInfo();
            ret.ProjectElements.ItemsRemoved += _ => ret.updateDatabaseObjectInfo();

            ret.updateDatabaseObjectInfo();

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
            Images.Add(new Image(textures, image.NewName));
        }

        /// <summary>
        /// Solo llamar cuando se quiera guardar
        /// </summary>
        /// <returns></returns>
        public string SaveableString()
        {
            if (FirstSave)
                returnableSaves--;
            DatabaseObject.ComunityStatus = CommunityStatus.Saved;

            StringBuilder builder = new StringBuilder();

            builder.AppendLine("[Info]");
            builder.AppendLine($"ChatRecommendation={ChatRecommendation}");
            builder.AppendLine($"Files={Images.Count}");
            foreach (var img in Images)
            {
                builder.AppendLine($"{img.ImageName}");
            }
            builder.AppendLine($"Image={Image.Value?.ImageName ?? "null"}");
            builder.AppendLine($"LastEdited={(DatabaseObject.LastEdited = DateTime.Now).ToUniversalTime():yyyyMMddHHmmssfff}");
            builder.AppendLine();

            builder.AppendLine("[Objects]");
            foreach (ProjectElement elem in ProjectElements)
            {
                builder.AppendLine($"{elem.ToSaveableString()}");
                builder.AppendLine();
            }

            return builder.ToString();
        }

        private bool parse(string[] lines)
        {
            bool isParsingObjects = false;

            ProjectElement parsingElement = null;

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
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
                    else
                    {
                        var tokens = line.Split('=');

                        if (tokens.Length != 2)
                            return false;

                        switch (tokens[0])
                        {
                            case "Images":
                                int amm = int.Parse(tokens[1]);
                                for (int j = i + amm; i < j; i++)
                                {
                                    var parts = lines[i + 1].Split('=');
                                    if (parts.Length != 2)
                                        return false;
                                    if (parts[1] == "null")
                                        continue;
                                    if (Images.First(im => im.ImageName == parts[1]) is var image && image != null)
                                        parsingElement.Images[parts[0]].Value = image;
                                    else
                                        return false;
                                }
                                break;
                            case "Size" when parsingElement is IHasSize size:
                                var xy = tokens[1].Split("|");
                                size.Size.Value = new Vector2(float.Parse(xy[0]), float.Parse(xy[1]));
                                break;
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(line))
                        continue;

                    var tokens = line.Split('=');

                    if (tokens.Length != 2)
                        return false;

                    switch (tokens[0])
                    {
                        case "ChatRecommendation":
                            ChatRecommendation = Enum.Parse<ChatRecommendation>(tokens[1]);
                            break;
                        case "Files":
                            int amm = int.Parse(tokens[1]);
                            for (int j = i + amm; i < j; i++)
                            {
                                Images.Add(new Image(textures, lines[i + 1]));
                            }
                            break;
                        case "Image":
                            if (tokens[1] == "null")
                                continue;
                            if (Images.First(im => im.ImageName == tokens[1]) is var image && image != null)
                                Image.Value = image;
                            break;
                    }
                }
            }

            return true;
        }
    }
}
