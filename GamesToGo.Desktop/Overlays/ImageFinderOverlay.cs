using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GamesToGo.Desktop.Database;
using GamesToGo.Desktop.Database.Models;
using GamesToGo.Desktop.Graphics;
using GamesToGo.Desktop.Project;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osuTK;
using DatabaseFile = GamesToGo.Desktop.Database.Models.File;

namespace GamesToGo.Desktop.Overlays
{
    [Cached]
    public class ImageFinderOverlay : OverlayContainer
    {
        [Resolved]
        private SplashInfoOverlay splashOverlay { get; set; }
        [Resolved]
        private GameHost host { get; set; }
        [Resolved]
        private Storage store { get; set; }
        [Resolved]
        private Context database { get; set; }
        private const float entries_per_row = 5;
        public const float ENTRY_WIDTH = (1920 - 100 - (entries_per_row - 1) * entry_spacing) / entries_per_row;
        private const float entry_spacing = 10;
        private const float entry_padding = 50;

        private WorkingProject project;

        private string filesPath;

        private Action<byte[]> selectionAction;

        private readonly string startingPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        private static string lastVisited;
        private FillFlowContainer<DirectoryButton> directoriesContainer;
        private FillFlowContainer<ImageButton> filesContainer;
        private BasicScrollContainer itemsScrollContainer;
        private SpriteText currentDirectoryText;

        [BackgroundDependencyLoader]
        private void load()
        {
            filesPath = store.GetFullPath("files/", true);

            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Colour4(100, 112, 206, 255),
                },
                itemsScrollContainer = new BasicScrollContainer
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    RelativeSizeAxes = Axes.Both,
                    ClampExtension = 30,
                    Padding = new MarginPadding { Top = 70 },
                    Children = new Drawable[]
                    {
                        new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(0, entry_padding / 3 * 2),
                            Padding = new MarginPadding { Top = entry_padding / 3 * 2 },
                            Children = new Drawable[]
                            {
                                directoriesContainer = new FillFlowContainer<DirectoryButton>
                                {
                                    Padding = new MarginPadding { Horizontal = entry_padding },
                                    Direction = FillDirection.Full,
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Anchor = Anchor.TopLeft,
                                    Origin = Anchor.TopLeft,
                                    Spacing = new Vector2(entry_spacing),
                                },
                                filesContainer = new FillFlowContainer<ImageButton>
                                {
                                    Padding = new MarginPadding { Horizontal = entry_padding },
                                    Direction = FillDirection.Full,
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Anchor = Anchor.TopLeft,
                                    Origin = Anchor.TopLeft,
                                    Spacing = new Vector2(entry_spacing),
                                },
                            },
                        },
                    },
                },
                new Container
                {
                    AutoSizeAxes = Axes.Y,
                    RelativeSizeAxes = Axes.X,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = new Colour4(130, 138, 230, 255),
                        },
                        currentDirectoryText = new SpriteText
                        {
                            Font = new FontUsage(size: 40),
                            Margin = new MarginPadding(15),
                        },
                        new IconButton(FontAwesome.Solid.Times, new Colour4(145, 151, 243, 255), backgroundColour: new Colour4(100, 112, 206, 255))
                        {
                            Action = Hide,
                            X = -10,
                        },
                    },
                },
            };
        }

        public void SelectImage(string path)
        {
            if (project != null)
                selectProjectImage(path);
            if (selectionAction != null)
                selectArrayImage(path);
        }

        private void selectArrayImage(string path)
        {
            selectionAction?.Invoke(System.IO.File.ReadAllBytes(path));
            Hide();
        }

        private void selectProjectImage(string path)
        {
            var finalName = GamesToGoEditor.HashBytes(System.IO.File.ReadAllBytes(path));
            var destinationPath = filesPath + finalName;
            DatabaseFile file;

            if (project.Images.Any(i => i.ImageName == finalName))
            {
                ShowError(@"Esta imagen ya ha sido agregada");
                return;
            }

            if (!store.Exists($"files/{finalName}"))
            {
                System.IO.File.Copy(path, destinationPath);
            }
            else if (GamesToGoEditor.HashBytes(System.IO.File.ReadAllBytes(destinationPath)) != finalName)
            {
                System.IO.File.Delete(destinationPath);
                System.IO.File.Copy(path, destinationPath);
            }

            if(database.Files.Any(f => f.NewName == finalName))
            {
                file = database.Files.FirstOrDefault(f => f.NewName == finalName);
            }
            else
            {
                database.Add(file = new DatabaseFile
                {
                    OriginalName = Path.GetFileName(path),
                    NewName = finalName,
                    Type = "image",
                });
            }

            database.Add(new FileRelation { File = file, Project = project.DatabaseObject });

            project.AddImage(file);
            Hide();
        }

        public void Show(WorkingProject importProject)
        {
            project = importProject;
            selectionAction = null;
            base.Show();
        }

        public void Show(Action<byte[]> onSelection)
        {
            selectionAction = onSelection;
            project = null;
            base.Show();
        }

        protected override void PopIn()
        {
            this.FadeIn(250);
            ChangeToDirectory(string.IsNullOrEmpty(lastVisited) ? startingPath : lastVisited);
        }

        protected override void PopOut()
        {
            this.FadeOut(250);

            directoriesContainer.Clear();
            filesContainer.Clear();

            host.Collect();
        }

        public void ShowError(string error)
        {
            splashOverlay.Show(error, new Colour4(44, 53, 119, 255));
        }

/*
        public void ChangeToParent()
        {
            ChangeToDirectory(Path.GetDirectoryName(lastVisited));
        }
*/

/*
        public void ChangeToSubdirectory(string subdirectory)
        {
            ChangeToDirectory(Path.Combine(lastVisited, subdirectory));
        }
*/

        public void ChangeToDirectory(string directory)
        {
            var newDirectories = new List<DirectoryButton>();
            var newFiles = new List<ImageButton>();

            if (string.IsNullOrEmpty(directory))
            {
                newDirectories.AddRange(Directory.GetLogicalDrives().Select(drive =>
                    new DirectoryButton(drive, DirectoryType.Drive)));
            }
            else
            {
                try
                {
                    newDirectories.AddRange(Directory.GetDirectories(directory, "*", new EnumerationOptions())
                        .Select(sub => new DirectoryButton(sub, DirectoryType.Directory)));

                    List<string> possibleFiles = new List<string>(Directory.GetFiles(directory, "*.png", new EnumerationOptions()));
                    possibleFiles.AddRange(Directory.GetFiles(directory, "*jpg", new EnumerationOptions()));

                    newFiles.AddRange(possibleFiles.Select(file => new ImageButton(file)));

                    newDirectories.Insert(0, new DirectoryButton(Path.GetDirectoryName(directory) ?? "", DirectoryType.ParentDirectory));
                }
                catch (IOException e)
                {
                    Logger.Log(@$"Error intentando obtener imagenes: {e.Message}", LoggingTarget.Runtime, LogLevel.Important);

                    ShowError(@$"No se puede acceder a {directory}");

                    return;
                }

                string newDirectory = Path.GetFileName(directory);

                currentDirectoryText.Text = string.IsNullOrEmpty(newDirectory) ? directory : newDirectory;
            }

            alreadyLoaded = false;
            lastVisited = directory;

            itemsScrollContainer.FadeOut(100);

            LoadComponentsAsync(newDirectories, nd => ensureAllLoaded(newDirectories, newFiles));
            LoadComponentsAsync(newFiles, nf => ensureAllLoaded(newDirectories, newFiles));
        }

        private bool alreadyLoaded;

        private void ensureAllLoaded(IReadOnlyCollection<DirectoryButton> dir, IReadOnlyCollection<ImageButton> file)
        {
            if (dir.Any(db => db.LoadState != LoadState.Ready) || file.Any(ib => ib.LoadState != LoadState.Ready) || alreadyLoaded)
                return;

            alreadyLoaded = true;

            directoriesContainer.Clear();
            filesContainer.Clear();

            directoriesContainer.AddRange(dir);
            filesContainer.AddRange(file);

            itemsScrollContainer.FadeIn(100);

            string target = Path.GetFileName(lastVisited);
            currentDirectoryText.Text = string.IsNullOrEmpty(lastVisited) ? @"Este equipo" : string.IsNullOrEmpty(target) ? lastVisited : target;

            host.Collect();
        }
    }
}
