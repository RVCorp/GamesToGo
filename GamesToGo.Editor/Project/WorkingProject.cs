using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GamesToGo.Common.Game;
using GamesToGo.Common.Online;
using GamesToGo.Editor.Database.Models;
using GamesToGo.Editor.Project.Actions;
using GamesToGo.Editor.Project.Arguments;
using GamesToGo.Editor.Project.Elements;
using GamesToGo.Editor.Project.Events;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osuTK;

namespace GamesToGo.Editor.Project
{
    public class WorkingProject
    {
        private const int current_file_version = 1;

        public static Dictionary<int, Type> AvailableEvents { get; } = new Dictionary<int, Type>();

        public static Dictionary<int, Type> AvailableActions { get; } = new Dictionary<int, Type>();

        public static Dictionary<int, Type> AvailableArguments { get; } = new Dictionary<int, Type>();

        private readonly TextureStore textures;
        public ProjectInfo DatabaseObject { get; }

        private int latestElementID = 1;

        private readonly BindableList<ProjectElement> projectElements = new BindableList<ProjectElement>();

        public readonly BindableList<EventAction> VictoryConditions = new BindableList<EventAction>();

        public readonly BindableList<EventAction> Turns = new BindableList<EventAction>();

        public readonly BindableList<EventAction> PreparationTurn = new BindableList<EventAction>();

        public IEnumerable<Card> ProjectCards => projectElements.OfType<Card>();

        public IEnumerable<Token> ProjectTokens => projectElements.OfType<Token>();

        public IEnumerable<Board> ProjectBoards => projectElements.OfType<Board>();

        public IEnumerable<Tile> ProjectTiles => projectElements.OfType<Tile>();

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
                project = new ProjectInfo {CreatorID = userID};
                returnableSaves = 1;
            }

            DatabaseObject = project;
            this.textures = textures;
        }

        public static WorkingProject Parse(ProjectInfo project, Storage store, TextureStore textures, APIController api)
        {
            WorkingProject ret = new WorkingProject(ref project, textures, api?.LocalUser.Value.ID ?? 0);

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

            ret.ProjectElements.CollectionChanged += (_, _) => ret.updateDatabaseObjectInfo();

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
            if (element.ID == 0)
                element.ID = latestElementID++;
            else if (element.ID >= latestElementID)
                latestElementID = element.ID + 1;

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
        public string SaveableString(CommunityStatus projectStatus)
        {
            if (FirstSave)
                returnableSaves--;

            DatabaseObject.CommunityStatus = projectStatus;

            var ret = stateString(true);

            lastSavedState = stateHash();

            return ret;
        }

        private string stateString(bool includeDate = false)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("[Info]");
            builder.AppendLine($"Version={current_file_version}");
            builder.AppendLine($"ChatRecommendation={ChatRecommendation}");
            builder.AppendLine($"Files={Images.Count}");

            foreach (var img in Images)
            {
                builder.AppendLine($"{img.ImageName}");
            }

            builder.AppendLine($"Image={Image.Value?.ImageName ?? "null"}");

            if (includeDate)
                builder.AppendLine($"LastEdited={(DatabaseObject.LastEdited = DateTime.Now).ToUniversalTime():yyyyMMddHHmmssfff}");

            builder.AppendLine($"VictoryConditions={VictoryConditions.Count}");

            foreach (var action in VictoryConditions)
            {
                builder.AppendLine($"{action}");
            }

            builder.AppendLine($"PreparationTurn={PreparationTurn.Count}");

            foreach (var action in PreparationTurn)
            {
                builder.AppendLine($"{action}");
            }

            builder.AppendLine($"Turns={Turns.Count}");

            foreach (var action in Turns)
            {
                builder.AppendLine($"{action}");
            }

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
                    int nextElement = elementQueue.Peek();
                    ProjectElement createdElement = projectElements.FirstOrDefault(e => e.ID == nextElement);

                    if (createdElement == null || createdElement.Type != elementedElement.NestedElementType)
                        return false;

                    createdElement = elementedElement.NestedElementType switch
                    {
                        ElementType.Token => (Token)createdElement,
                        ElementType.Card => (Card)createdElement,
                        ElementType.Tile => (Tile)createdElement,
                        ElementType.Board => (Board)createdElement,
                        _ => null,
                    };

                    if (createdElement == null || createdElement.Parent != null)
                        return false;

                    createdElement.Parent = element;

                    elementedElement.Elements.Add(createdElement);
                    elementQueue.Dequeue();
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

                        parsingElement = Enum.Parse<ElementType>(idents[0]) switch
                        {
                            ElementType.Token => new Token(),
                            ElementType.Card => new Card(),
                            ElementType.Tile => new Tile(),
                            ElementType.Board => new Board(),
                            _ => null,
                        };

                        if (parsingElement == null)
                            return false;

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
                                    var parts = lines[i + 1].Split('=', ':');

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
                                orientedElement.DefaultOrientation.Value = Enum.Parse<ElementOrientation>(tokens[1]);

                                break;
                            }
                            case "Side" when parsingElement is IHasSideVisible sidedElement:
                            {
                                sidedElement.DefaultSide.Value = Enum.Parse<ElementSideVisible>(tokens[1]);

                                break;
                            }
                            case "Privacy" when parsingElement is IHasPrivacy privacySetElement:
                            {
                                privacySetElement.DefaultPrivacy.Value = Enum.Parse<ElementPrivacy>(tokens[1]);

                                break;
                            }
                            case "Position" when parsingElement is IHasPosition position:
                            {
                                var xy = tokens[1].Split("|");
                                position.Position.Value = new Vector2(float.Parse(xy[0]), float.Parse(xy[1]));

                                break;
                            }
                            case "Arrangement" when parsingElement is IHasLogicalArrangement arrangedElement:
                            {
                                var xy = tokens[1].Split("|");
                                arrangedElement.Arrangement.Value = new Vector2(float.Parse(xy[0]), float.Parse(xy[1]));

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

                                    var eventArgs = args.Split(',', StringSplitOptions.RemoveEmptyEntries);

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

                                        var actionArgs = divideArguments(arguments).ToArray();

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
                        case "VictoryConditions":
                            int vicAmm = int.Parse(tokens[1]);

                            for (int j = i + vicAmm; i < j; i++)
                            {
                                VictoryConditions.Add(populateAction(lines[i + 1]));
                            }

                            break;
                        case "PreparationTurn":
                            int prepTurnAmm = int.Parse(tokens[1]);

                            for (int j = i + prepTurnAmm; i < j; i++)
                            {
                                PreparationTurn.Add(populateAction(lines[i + 1]));
                            }

                            break;
                        case "Turns":
                            int turnAmm = int.Parse(tokens[1]);

                            for (int j = i + turnAmm; i < j; i++)
                            {
                                Turns.Add(populateAction(lines[i + 1]));
                            }

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

            if (toBeArgument is IHasResult resolvedArgument)
            {
                if (!string.IsNullOrEmpty(argText))
                    resolvedArgument.Result.Value = int.Parse(argText);

                return toBeArgument;
            }

            if (string.IsNullOrEmpty(argText))
                return toBeArgument;

            var subArgs = divideArguments(argText).ToArray();

            for (int i = 0; i < subArgs.Length; i++)
            {
                toBeArgument.Arguments[i].Value = populateArgument(subArgs[i]);
            }

            return toBeArgument;
        }

        private static EventAction populateAction(string text)
        {
            var action = text.Split('|');
            int actType = divideLine(action[1], out string arguments);

            var actionArgs = divideArguments(arguments).ToArray();

            if (!(Activator.CreateInstance(AvailableActions[actType]) is EventAction toBeAction))
                return null;

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

            return toBeAction;
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

        private static IEnumerable<string> divideArguments(string line)
        {
            int parenthesisOpenCount = 0, parenthesisCloseCount = 0, lastStart = 0;

            for (int i = 0; i < line.Length; i++)
            {
                switch (line[i])
                {
                    case '(':
                        parenthesisOpenCount++;

                        break;
                    case ')':
                        parenthesisCloseCount++;

                        break;
                    case ',':
                        if (parenthesisOpenCount == parenthesisCloseCount)
                        {
                            if (i != lastStart)
                                yield return line.Substring(lastStart, i - lastStart);
                            lastStart = i + 1;
                        }

                        break;
                }
            }

            if (parenthesisOpenCount == parenthesisCloseCount)
                yield return line.Substring(lastStart);
        }


        /// <summary>
        /// This assumes CrawlEventsForReferences has been called and user is ok with possible references being deleted.
        /// </summary>
        /// <param name="toDeleteElement"></param>
        public void RemoveElement(ProjectElement toDeleteElement)
        {
            if (toDeleteElement is IHasElements elemented)
            {
                while (elemented.Elements.Any())
                {
                    var sub = elemented.Elements[0];
                    RemoveElement(sub);
                    elemented.Elements.Remove(sub);
                }
            }

            crawlAndDeleteReferences(toDeleteElement);
            projectElements.Remove(toDeleteElement);
        }

        private void crawlAndDeleteReferences(ProjectElement toDeleteElement)
        {
            Turns.ForEach(t => t.DeleteReferenceTo(toDeleteElement));
            VictoryConditions.ForEach(vc => vc.DeleteReferenceTo(toDeleteElement));
            PreparationTurn.ForEach(vc => vc.DeleteReferenceTo(toDeleteElement));

            projectElements.ForEach(e =>
            {
                if (e is IHasEvents ee) ee.DeleteReferenceTo(toDeleteElement);
            });
        }

        public bool CrawlEventsForReferences(ProjectElement element)
        {
            return Turns.Any(t => t.HasReferenceTo(element)) ||
                   VictoryConditions.Any(vc => vc.HasReferenceTo(element)) ||
                   PreparationTurn.Any(vc => vc.HasReferenceTo(element)) ||
                   projectElements.Any(e => e is IHasEvents ee && ee.HasReferenceTo(element));
        }
    }
}
