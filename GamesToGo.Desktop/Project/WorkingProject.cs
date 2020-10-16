using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GamesToGo.Desktop.Database.Models;
using GamesToGo.Desktop.Online;
using GamesToGo.Desktop.Project.Actions;
using GamesToGo.Desktop.Project.Arguments;
using GamesToGo.Desktop.Project.Elements;
using GamesToGo.Desktop.Project.Events;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osuTK;

namespace GamesToGo.Desktop.Project
{
    public class WorkingProject
    {
        public static Dictionary<int, Type> AvailableEvents { get; } = new Dictionary<int, Type>();

        public static Dictionary<int, Type> AvailableActions { get; } = new Dictionary<int, Type>();

        public static Dictionary<int, Type> AvailableArguments { get; } = new Dictionary<int, Type>();

        private readonly TextureStore textures;
        public ProjectInfo DatabaseObject { get; }

        private int latestElementID = 1;

        private readonly BindableList<ProjectElement> projectElements = new BindableList<ProjectElement>();

        private IEnumerable<Card> ProjectCards => projectElements.OfType<Card>();

        private IEnumerable<Token> ProjectTokens => projectElements.OfType<Token>();

        private IEnumerable<Board> ProjectBoards => projectElements.OfType<Board>();

        private IEnumerable<Tile> ProjectTiles => projectElements.OfType<Tile>();

        public IBindableList<ProjectElement> ProjectElements => projectElements;

        public readonly BindableList<Image> Images = new BindableList<Image>();

        public Bindable<Image> Image { get; } = new Bindable<Image>();

        public ChatRecommendation ChatRecommendation { get; set; }

        private int returnableSaves;

        public bool FirstSave => returnableSaves > 0;

        private string lastSavedState { get; set; }

        public bool HasUnsavedChanges => lastSavedState != stateHash();

        private WorkingProject(ref ProjectInfo project, TextureStore textures, int userID)
        {
            if (project == null)
            {
                project = new ProjectInfo { CreatorID = userID };
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
                    if (!ret.postParse())
                        return null;
                }
                catch
                {
                    if (Debugger.IsAttached)
                        throw;

                    return null;
                }

                if (project.Relations != null)
                {
                    if (project.Relations.Count != ret.Images.Count)
                        return null;

                    if (project.Relations.Any(image => ret.Images.All(im => im.ImageName != image.File.NewName)))
                    {
                        return null;
                    }
                }
            }

            ret.ProjectElements.CollectionChanged += (_, __) => ret.updateDatabaseObjectInfo();

            ret.updateDatabaseObjectInfo();

            ret.lastSavedState = ret.stateHash();

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

        private string stateHash()
        {
            return GamesToGoEditor.HashBytes(Encoding.UTF8.GetBytes(stateString() + JsonConvert.SerializeObject(DatabaseObject)));
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

            var ret = stateString(true);

            lastSavedState = stateHash();

            return ret;
        }

        private string stateString(bool includeDate = false)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("[Info]");
            builder.AppendLine($"ChatRecommendation={ChatRecommendation}");
            builder.AppendLine($"Files={Images.Count}");
            foreach (var img in Images)
            {
                builder.AppendLine($"{img.ImageName}");
            }
            builder.AppendLine($"Image={Image.Value?.ImageName ?? "null"}");

            if (includeDate)
                builder.AppendLine($"LastEdited={(DatabaseObject.LastEdited = DateTime.Now).ToUniversalTime():yyyyMMddHHmmssfff}");

            builder.AppendLine();

            builder.AppendLine("[Objects]");
            foreach (ProjectElement elem in ProjectElements)
            {
                builder.AppendLine(elem.ToSaveableString());
            }

            return builder.ToString().Trim('\n', '\r');
        }

        private bool postParse()
        {
            foreach (var element in projectElements)
            {
                if (!(element is IHasElements elementedElement)) continue;
                var elementQueue = elementedElement.PendingElements;
                while (elementQueue.Count > 0)
                {
                    int nextElement = elementQueue.Dequeue();
                    switch (elementedElement.NestedElementType)
                    {
                        case ElementType.Token:
                            if (ProjectTokens.All(b => b.ID != nextElement))
                                return false;
                            elementedElement.Elements.Add(ProjectTokens.First(b => b.ID == nextElement));
                            break;
                        case ElementType.Card:
                            if (ProjectCards.All(b => b.ID != nextElement))
                                return false;
                            elementedElement.Elements.Add(ProjectCards.First(b => b.ID == nextElement));
                            break;
                        case ElementType.Tile:
                            if (ProjectTiles.All(b => b.ID != nextElement))
                                return false;
                            elementedElement.Elements.Add(ProjectTiles.First(b => b.ID == nextElement));
                            break;
                        case ElementType.Board:
                            if (ProjectBoards.All(b => b.ID != nextElement))
                                return false;
                            elementedElement.Elements.Add(ProjectBoards.First(b => b.ID == nextElement));
                            break;
                    }
                }
            }

            return true;
        }

        private bool parse(IReadOnlyList<string> lines)
        {
            bool isParsingObjects = false;

            ProjectElement parsingElement = null;

            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.StartsWith('['))
                {
                    isParsingObjects = line.Trim('[', ']') switch
                    {
                        "Info" => false,
                        "Objects" => true,
                        _ => isParsingObjects,
                    };

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
                        switch (Enum.Parse<ElementType>(idents[0]))
                        {
                            case ElementType.Token:
                                parsingElement = new Token();
                                break;
                            case ElementType.Card:
                                parsingElement = new Card();
                                break;
                            case ElementType.Tile:
                                parsingElement = new Tile();
                                break;
                            case ElementType.Board:
                                parsingElement = new Board();
                                break;
                            default:
                                return false;
                        }
                        parsingElement.ID = int.Parse(idents[1]);
                        parsingElement.Name.Value = idents[2];
                    }
                    else
                    {
                        var tokens = line.Split('=');

                        if (tokens.Length != 2)
                            return false;

                        switch (tokens[0])
                        {
                            case "Images":
                            {
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
                            }
                            case "Desc":
                            {
                                parsingElement.Description.Value = tokens[1];
                                break;
                            }
                            case "Size" when parsingElement is IHasSize size:
                            {
                                var xy = tokens[1].Split("|");
                                size.Size.Value = new Vector2(float.Parse(xy[0]), float.Parse(xy[1]));
                                break;
                            }
                            case "SubElems" when parsingElement is IHasElements elementedElement:
                            {
                                int amm = int.Parse(tokens[1]);
                                for (int j = i + amm; i < j; i++)
                                {
                                    elementedElement.QueueElement(int.Parse(lines[i + 1]));
                                }
                                break;
                            }
                            case "Orient" when parsingElement is IHasOrientation orientedElement:
                            {
                                orientedElement.DefaultOrientation = Enum.Parse<ElementOrientation>(tokens[1]);
                                break;
                            }
                            case "Privacy" when parsingElement is IHasPrivacy privacySetElement:
                            {
                                privacySetElement.DefaultPrivacy = Enum.Parse<ElementPrivacy>(tokens[1]);
                                break;
                            }
                            case "Events" when parsingElement is IHasEvents eventedElement:
                            {
                                int amm = int.Parse(tokens[1]);
                                for (int j = i + amm; i < j; i++)
                                {
                                    var splits = lines[i + 1].Split('|');
                                    int type = divideLine(splits[1], out string args);

                                    ProjectEvent toBeEvent = Activator.CreateInstance(AvailableEvents[type]) as ProjectEvent;
                                    toBeEvent.ID = int.Parse(splits[0]);
                                    toBeEvent.Condition.Value = populateArgument(splits[4]);
                                    toBeEvent.Name.Value = splits[2];
                                    toBeEvent.Priority.Value = int.Parse(splits[3]);

                                    var eventArgs = args.Split(',');

                                    for (int argIndex = 0; argIndex < eventArgs.Length; argIndex++)
                                    {
                                        toBeEvent.Arguments[argIndex].Value = populateArgument(eventArgs[argIndex]);
                                    }

                                    int actAmm = int.Parse(splits[5]);
                                    j += actAmm;

                                    for (int k = i + actAmm; i < k; i++)
                                    {
                                        var action = lines[i + 2].Split('|');
                                        int actType = divideLine(action[1], out string arguments);

                                        var actionArgs = arguments.Split(',');

                                        EventAction toBeAction = Activator.CreateInstance(AvailableActions[actType]) as EventAction;

                                        if (toBeAction.Condition.Value != null)
                                            return false;

                                        for (int argIndex = 0; argIndex < actionArgs.Length; argIndex++)
                                        {
                                            toBeAction.Arguments[argIndex].Value = populateArgument(actionArgs[argIndex]);
                                        }

                                        try
                                        {
                                            toBeAction.Condition.Value = populateArgument(action[2]);
                                        }
                                        catch (IndexOutOfRangeException)
                                        {
                                            toBeAction.Condition.Value = null;
                                        }

                                        toBeEvent.Actions.Add(toBeAction);
                                    }

                                    eventedElement.Events.Add(toBeEvent);
                                }
                                break;
                            }
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
                            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                            if (tokens[1] == @"Presential")
                                ChatRecommendation = ChatRecommendation.FaceToFace;
                            else
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

            if (parsingElement != null)
                AddElement(parsingElement);

            return true;
        }

        /// <summary>
        /// Obtiene el argumento y argumentos anidados para el texto introducido
        /// </summary>
        /// <param name="text">el texto del argumento a analizar</param>
        /// <returns>El argumento con sus argumentos anidados formados</returns>
        private static Argument populateArgument(string text)
        {
            if (string.IsNullOrEmpty(text) || text == "null")
                return null;

            var type = divideLine(text, out string argText);

            if (!(Activator.CreateInstance(AvailableArguments[type]) is Argument toBeArgument))
                return null;

            if (toBeArgument.HasResult)
            {
                if (!string.IsNullOrEmpty(argText))
                    toBeArgument.Result = int.Parse(argText);

                return toBeArgument;
            }

            if (string.IsNullOrEmpty(argText))
                return toBeArgument;

            var subArgs = argText.Split(',');

            for (int i = 0; i < subArgs.Length; i++)
            {
                toBeArgument.Arguments[i].Value = populateArgument(subArgs[i]);
            }

            return toBeArgument;
        }

        /// <summary>
        /// Divide una linea de evento, accion o argumento en sus partes
        /// </summary>
        /// <param name="line">La linea a dividir</param>
        /// <param name="arguments">Salida de la linea de argumentos</param>
        /// <returns>ID del evento, argumento o accion, -1 si no se encontró</returns>
        private static int divideLine(string line, out string arguments)
        {
            string args = line.Split('(', 2)[1];
            arguments = args.Substring(0, args.Length - 1);
            return int.Parse(line.Split('(')[0]);
        }
    }
}
