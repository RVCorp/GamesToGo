using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using GamesToGo.Desktop.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Platform;
using osuTK;
using osuTK.Graphics;
using osu.Framework.Logging;
using osu.Framework.Graphics.Textures;

namespace GamesToGo.Desktop.Overlays
{
    public class ImageFinderOverlay : OverlayContainer
    {
        private GameHost host;
        private const float entries_per_row = 5;
        public const float ENTRY_WIDTH = (1920 - 100 - ((entries_per_row - 1) * entry_spacing)) / entries_per_row;
        private const float entry_spacing = 10;
        private const float entry_padding = 50;

        private readonly string startingPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
        private static string lastVisited;
        private FillFlowContainer<DirectoryButton> directoriesContainer;
        private FillFlowContainer<ImageButton> filesContainer;
        private BasicScrollContainer itemsScrollContainer;
        private SpriteText currentDirectoryText;
        private Container errorContainer;
        private SpriteText errorText;

        private DependencyContainer dependencies;
        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            return dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
        }

        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            this.host = host;
            dependencies.Cache(this);

            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Color4(100, 112, 206, 255),
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
                            }
                        },
                    }
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
                            Colour = new Color4(130, 138, 230, 255)
                        },
                        currentDirectoryText = new SpriteText
                        {
                            Font = new FontUsage(size: 40),
                            Margin = new MarginPadding(15),
                        },
                    }
                },
                new Container
                {
                    RelativeSizeAxes = Axes.X,
                    Height = 50,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.TopCentre,
                    Child = errorContainer = new Container
                    {
                        RelativePositionAxes = Axes.Y,
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                Colour = new Color4(44, 53, 119, 255),
                                RelativeSizeAxes = Axes.Both,
                            },
                            errorText = new SpriteText
                            {
                                Margin = new MarginPadding(12),
                                Font = new FontUsage(size: 30),
                                Truncate = true,
                                RelativeSizeAxes = Axes.X,
                            }
                        }
                    }
                },
            };
        }

        protected override void PopIn()
        {
            this.FadeIn(250);
            if (string.IsNullOrEmpty(lastVisited))
                ChangeToDirectory(startingPath);
            else
                ChangeToDirectory(lastVisited);
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
            errorText.Text = error;

            errorContainer.MoveToY(-1, 400, Easing.OutCubic)
                .Delay(5000)
                .MoveToY(0, 400, Easing.OutCubic);
        }

        public void ChangeToParent()
        {
            ChangeToDirectory(Path.GetDirectoryName(lastVisited));
        }

        public void ChangeToSubdirectory(string subdirectory)
        {
            ChangeToDirectory(Path.Combine(lastVisited, subdirectory));
        }

        public void ChangeToDirectory(string directory)
        {
            if (lastVisited == directory)
                return;

            var newDirectories = new List<DirectoryButton>();
            var newFiles = new List<ImageButton>();

            if (string.IsNullOrEmpty(directory))
            {
                foreach (string drive in Directory.GetLogicalDrives())
                    newDirectories.Add(new DirectoryButton(drive, DirectoryType.Drive));
            }
            else
            {
                try
                {
                    foreach (var sub in Directory.GetDirectories(directory, "*", new EnumerationOptions() { }))
                        newDirectories.Add(new DirectoryButton(sub, DirectoryType.Directory));

                    foreach (var file in Directory.GetFiles(directory, "*.png", new EnumerationOptions() { }))
                        newFiles.Add(new ImageButton(file));

                    newDirectories.Insert(0, new DirectoryButton(Path.GetDirectoryName(directory) ?? "", DirectoryType.ParentDirectory));
                }
                catch (IOException e)
                {
                    Logger.Log($"Error intentando obtener imagenes: {e.Message}", LoggingTarget.Runtime, LogLevel.Important);

                    ShowError($"No se puede acceder a {directory}");

                    return;
                }

                string newDirectory = Path.GetFileName(directory);

                currentDirectoryText.Text = string.IsNullOrEmpty(newDirectory) ? directory : newDirectory;
            }

            alreadyloaded = false;
            lastVisited = directory;

            itemsScrollContainer.FadeOut(100);


            LoadComponentsAsync(newDirectories, nd => ensureAllLoaded(nd, newFiles));
            LoadComponentsAsync(newFiles, nf => ensureAllLoaded(newDirectories, nf));
        }

        private bool alreadyloaded = false;

        private void ensureAllLoaded(IEnumerable<DirectoryButton> dir, IEnumerable<ImageButton> file)
        {
            if (dir.Any(db => db.LoadState != LoadState.Ready) || file.Any(ib => ib.LoadState != LoadState.Ready) || alreadyloaded)
                return;

            alreadyloaded = true;

            directoriesContainer.Clear();
            filesContainer.Clear();

            directoriesContainer.AddRange(dir);
            filesContainer.AddRange(file);

            itemsScrollContainer.FadeIn(100);
            
            string target = Path.GetFileName(lastVisited);
            currentDirectoryText.Text = string.IsNullOrEmpty(lastVisited) ? "Este equipo" : string.IsNullOrEmpty(target) ? lastVisited : target;

            host.Collect();
        }
    }
}
