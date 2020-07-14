﻿using System;
using System.IO;
using System.Text;
using GamesToGo.Desktop.Database.Models;
using GamesToGo.Desktop.Graphics;
using GamesToGo.Desktop.Overlays;
using GamesToGo.Desktop.Project;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;
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
        private WorkingProject workingProject;
        private EditorTabChanger tabsBar;


        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            return dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
        }

        public ProjectEditor(WorkingProject project)
        {
            workingProject = project;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures, Storage store, Context database, SplashInfoOverlay splashOverlay)
        {
            this.store = store;
            this.database = database;
            this.splashOverlay = splashOverlay;

            if (workingProject == null)
            {
                workingProject = WorkingProject.Parse(new ProjectInfo { Name = "Nuevo Proyecto" }, store, textures, database);
                SaveProject(false);
            }

            InternalChildren = new[]
            {
                new Container
                {
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
                    },
                },
            };

            tabsBar.Current.ValueChanged += changeEditorScreen;
            CurrentEditingElement.ValueChanged += _ => tabsBar.Current.Value = EditorScreenOption.Objetos;

            dependencies.Cache(workingProject);
            dependencies.Cache(this);

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
    }
}
