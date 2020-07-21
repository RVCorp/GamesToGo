using System;
using System.IO;
using System.Text;
using GamesToGo.Desktop.Database.Models;
using GamesToGo.Desktop.Graphics;
using GamesToGo.Desktop.Overlays;
using GamesToGo.Desktop.Project;
using Microsoft.EntityFrameworkCore;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK.Graphics;
using DatabaseFile = GamesToGo.Desktop.Database.Models.File;

namespace GamesToGo.Desktop.Screens
{
    public class ProjectEditor : Screen
    {
        private Screen currentScreen;

        private ScreenStack screenContainer;

        private readonly Bindable<ProjectElement> currentEditingElement = new Bindable<ProjectElement>();

        public IBindable<ProjectElement> CurrentEditingElement => currentEditingElement;

        private DependencyContainer dependencies;
        private Storage store;
        private Context database;
        private SplashInfoOverlay splashOverlay;
        private MultipleOptionOverlay optionOverlay;
        private WorkingProject workingProject;
        private EditorTabChanger tabsBar;
        private ImageFinderOverlay imageFinder;
        private ImagePickerOverlay imagePicker;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            return dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
        }

        public ProjectEditor(WorkingProject project)
        {
            workingProject = project;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures, Storage store, Context database, SplashInfoOverlay splashOverlay, MultipleOptionOverlay optionOverlay)
        {
            this.store = store;
            this.database = database;
            this.splashOverlay = splashOverlay;
            this.optionOverlay = optionOverlay;

            if (workingProject == null)
            {
                workingProject = WorkingProject.Parse(new ProjectInfo { Name = "Nuevo Proyecto" }, store, textures, database);
                SaveProject(false);
            }

            dependencies.Cache(workingProject);
            dependencies.Cache(this);

            InternalChildren = new[]
            {
                new Container
                {
                    Depth = 4,
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding
                    {
                        Top = 30,
                    },
                    Child = screenContainer = new ScreenStack
                    {
                        RelativeSizeAxes = Axes.Both,
                    },
                },
                new Container
                {
                    Depth = 3,
                    RelativeSizeAxes = Axes.X,
                    Height = 30,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.Gray
                        },
                        tabsBar = new EditorTabChanger(),
                        new CloseButton
                        {
                            Action = confirmClose
                        },
                    },
                },
                imageFinder = new ImageFinderOverlay
                {
                    Depth = 1
                }
            };

            tabsBar.Current.ValueChanged += changeEditorScreen;
            CurrentEditingElement.ValueChanged += _ => tabsBar.Current.Value = EditorScreenOption.Objetos;

            dependencies.Cache(imageFinder);

            AddInternal(imagePicker = new ImagePickerOverlay
            {
                Depth = 2
            });
            dependencies.Cache(imagePicker);

            tabsBar.Current.Value = EditorScreenOption.Inicio;
        }

        public void SelectElement(ProjectElement element)
        {
            currentEditingElement.Value = element;
        }

        public void SaveProject(bool showSplashConfirmation = true)
        {
            string fileString = workingProject.SaveableString();
            string newFileName;
            using (MemoryStream stream = new MemoryStream())
            {
                var sw = new StreamWriter(stream, new UnicodeEncoding());

                sw.Write(fileString);
                sw.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                newFileName = GamesToGoEditor.HashBytes(stream.ToArray());
                sw.Dispose();
            }

            using (Stream fileStream = store.GetStream($"files/{newFileName}", FileAccess.Write, FileMode.Create))
            {
                var sw = new StreamWriter(fileStream, new UnicodeEncoding());

                sw.Write(fileString);
                sw.Flush();
                sw.Dispose();
            }

            if (workingProject.DatabaseObject.File == null)
            {
                workingProject.DatabaseObject.File = new DatabaseFile
                {
                    OriginalName = "",
                    Type = "project",
                };

                database.Add(workingProject.DatabaseObject);
            }
            else
            {
                store.Delete($"files/{workingProject.DatabaseObject.File.NewName}");
            }

            workingProject.DatabaseObject.File.NewName = newFileName;

            database.SaveChanges();

            Random random = new Random();

            if (showSplashConfirmation)
                splashOverlay.Show("Se ha guardado el proyecto localmente", new Color4(randomNumber(), randomNumber(), randomNumber(), 255)/*new Color4(80, 80, 80, 255)*/);

            byte randomNumber()
            {
                return (byte)(random.NextDouble() * 255);
            }
        }

        public void AddElement(ProjectElement element, bool startEditing)
        {
            workingProject.AddElement(element);
            if (startEditing)
                SelectElement(element);
        }

        private void changeEditorScreen(ValueChangedEvent<EditorScreenOption> value)
        {
            currentScreen?.Exit();
            switch (value.NewValue)
            {
                case EditorScreenOption.Archivo:
                    currentScreen = new ProjectFileScreen();
                    break;
                case EditorScreenOption.Inicio:
                    currentScreen = new ProjectHomeScreen();
                    break;
                case EditorScreenOption.Objetos:
                    currentScreen = new ProjectObjectScreen();
                    break;
            }

            LoadComponentAsync(currentScreen, screenContainer.Push);
        }

        private void confirmClose()
        {
            optionOverlay.Show("¿Estás seguro que quieres volver al menú principal?",
                new OptionItem[]
                {
                    new OptionItem
                    {
                        Action = () =>
                        {
                            SaveProject(false);
                            this.Exit();
                        },
                        Text = "Guardar y salir",
                        Type = OptionType.Additive,
                    },
                    new OptionItem
                    {
                        Action = () =>
                        {
                            discardChanges();
                            this.Exit();
                        },
                        Text = "Salir sin guardar",
                        Type = OptionType.Destructive,
                    },
                    new OptionItem
                    {
                        Text = "Volver al editor",
                        Type = OptionType.Neutral
                    }
                });
        }

        private void discardChanges()
        {
            foreach (var entry in database.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }

        private class CloseButton : Button
        {
            private Box hoverBox;

            public CloseButton()
            {
                RelativeSizeAxes = Axes.Y;
                AutoSizeAxes = Axes.X;
                Anchor = Anchor.CentreRight;
                Origin = Anchor.CentreRight;
                Children = new Drawable[]
                {
                    hoverBox = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Red,
                        Alpha = 0,
                    },
                    new SpriteText
                    {
                        Margin = new MarginPadding { Horizontal = 5 },
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Text = "Volver al inicio",
                    }
                };
            }

            protected override bool OnHover(HoverEvent e)
            {
                hoverBox.FadeIn(125);
                return base.OnHover(e);
            }

            protected override void OnHoverLost(HoverLostEvent e)
            {
                hoverBox.FadeOut(125);
                base.OnHoverLost(e);
            }
        }
    }
}
