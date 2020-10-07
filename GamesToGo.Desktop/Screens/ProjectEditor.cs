using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GamesToGo.Desktop.Database;
using GamesToGo.Desktop.Database.Models;
using GamesToGo.Desktop.Graphics;
using GamesToGo.Desktop.Online;
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
using DatabaseFile = GamesToGo.Desktop.Database.Models.File;

namespace GamesToGo.Desktop.Screens
{
    [Cached]
    public class ProjectEditor : Screen
    {
        private Screen currentScreen;

        private ScreenStack screenContainer;

        private readonly Bindable<ProjectElement> currentEditingElement = new Bindable<ProjectElement>();

        public IBindable<ProjectElement> CurrentEditingElement => currentEditingElement;

        [Resolved]
        private Storage store { get; set; }

        [Resolved]
        private Context database { get; set; }

        [Resolved]
        private SplashInfoOverlay splashOverlay { get; set; }

        [Resolved]
        private MultipleOptionOverlay optionOverlay { get; set; }

        [Resolved]
        private APIController api { get; set; }

        [Cached]
        private WorkingProject workingProject;
        private List<FileRelation> initialRelations;
        private EditorTabChanger tabsBar;
        [Cached]
        private ImagePickerOverlay imagePicker = new ImagePickerOverlay { Depth = 2 };

        public ProjectEditor(WorkingProject project)
        {
            workingProject = project;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            if (workingProject == null)
            {
                workingProject = WorkingProject.Parse(null, store, textures, api);
                SaveProject(false);
            }

            initialRelations = workingProject.DatabaseObject.Relations == null ? null : new List<FileRelation>(workingProject.DatabaseObject.Relations);

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
                            Colour = Colour4.Gray,
                        },
                        tabsBar = new EditorTabChanger(),
                        new CloseButton
                        {
                            Action = confirmClose,
                        },
                    },
                },
            };

            tabsBar.Current.ValueChanged += changeEditorScreen;
            CurrentEditingElement.ValueChanged += _ => tabsBar.Current.Value = tabsBar.Current.Value switch
            {
                EditorScreenOption.Objetos => EditorScreenOption.Objetos,
                EditorScreenOption.Eventos => EditorScreenOption.Eventos,
                _ => EditorScreenOption.Objetos,
            };

            AddInternal(imagePicker);

            tabsBar.Current.Value = EditorScreenOption.Inicio;
        }

        public void SelectElement(ProjectElement element)
        {
            currentEditingElement.Value = element;
        }

        public void SaveProject(bool showSplashConfirmation = true)
        {
            database.SaveChanges();

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
                    Type = "project",
                };

                database.Add(workingProject.DatabaseObject);
            }
            else
            {
                store.Delete($"files/{workingProject.DatabaseObject.File.NewName}");
            }

            workingProject.DatabaseObject.File.NewName = newFileName;
            workingProject.DatabaseObject.ImageRelationID = workingProject.Image.Value == null ? null :
                (int?)workingProject.DatabaseObject.Relations.First(r => r.File.NewName == workingProject.Image.Value.ImageName).RelationID;

            database.SaveChanges();

            initialRelations = workingProject.DatabaseObject.Relations == null ? null : new List<FileRelation>(workingProject.DatabaseObject.Relations);

            Random random = new Random();

            if (showSplashConfirmation)
                splashOverlay.Show(@"Se ha guardado el proyecto localmente", new Colour4(randomNumber(), randomNumber(), randomNumber(), 255)/*new Colour4(80, 80, 80, 255)*/);

            byte randomNumber()
            {
                return (byte)(random.NextDouble() * 255);
            }
        }

        public void UploadProject()
        {
            SaveProject(false);
            splashOverlay.Show(@"Proyecto guardado localmente, subiendo al servidor...", Colour4.ForestGreen);
            var req = new UploadGameRequest(workingProject.DatabaseObject, store);
            req.Failure += e =>
            {
                splashOverlay.Show(@"Hubo un problema al subir el proyecto al servidor", Colour4.DarkRed);
            };
            req.Success += res =>
            {
                workingProject.DatabaseObject.OnlineProjectID = res.OnlineID;
                splashOverlay.Show(@"Proyecto subido al servidor, ahora puedes acceder a el desde cualquier lugar", Colour4.ForestGreen);
                database.SaveChanges();
            };
            api.Queue(req);
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

            currentScreen = value.NewValue switch
            {
                EditorScreenOption.Archivo => new ProjectFileScreen(),
                EditorScreenOption.Inicio => new ProjectHomeScreen(),
                EditorScreenOption.Objetos => new ProjectObjectScreen(),
                EditorScreenOption.Eventos => new ProjectEventsScreen(),
                _ => currentScreen,
            };

            LoadComponentAsync(currentScreen!, screenContainer.Push);
        }

        private void confirmClose()
        {
            if (!workingProject.HasUnsavedChanges)
            {
                this.Exit();
                return;
            }

            optionOverlay.Show(@"¿Estás seguro que quieres volver al menú principal?", new[]
            {
                new OptionItem
                {
                    Action = () =>
                    {
                        SaveProject(false);
                        this.Exit();
                    },
                    Text = @"Guardar y salir",
                    Type = OptionType.Additive,
                },
                new OptionItem
                {
                    Action = () =>
                    {
                        discardChanges();
                        this.Exit();
                    },
                    Text = @"Salir sin guardar",
                    Type = OptionType.Destructive,
                },
                new OptionItem
                {
                    Text = @"Volver al editor",
                    Type = OptionType.Neutral,
                },
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

            if (workingProject.DatabaseObject.Relations != null)
            {
                workingProject.DatabaseObject.Relations.Clear();
                workingProject.DatabaseObject.Relations = initialRelations == null ? null : new List<FileRelation>(initialRelations);
            }

            if (workingProject.FirstSave)
            {
                workingProject.DatabaseObject.File = null;
                workingProject.DatabaseObject.ImageRelation = null;
                database.Remove(workingProject.DatabaseObject);
            }

            database.SaveChanges();
        }

        private class CloseButton : Button
        {
            private Box hoverBox;

            [BackgroundDependencyLoader]
            private void load()
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
                        Colour = Colour4.Red,
                        Alpha = 0,
                    },
                    new SpriteText
                    {
                        Margin = new MarginPadding { Horizontal = 5 },
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Text = @"Volver al inicio",
                    },
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
