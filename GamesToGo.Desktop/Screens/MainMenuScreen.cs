using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK.Graphics;
using osuTK;
using GamesToGo.Desktop.Graphics;
using GamesToGo.Desktop.Database.Models;
using GamesToGo.Desktop.Project;
using System.Linq;
using osu.Framework.Platform;
using GamesToGo.Desktop.Online;
using GamesToGo.Desktop.Overlays;
using System.IO;
using Ionic.Zip;
using osu.Framework.Graphics.Textures;

namespace GamesToGo.Desktop.Screens
{
    /// <summary>
    /// Pantalla del menu principal, muestra los proyectos del usuario, su perfil, y un modal para cerrar sesión. (WIP)
    /// </summary>
    public class MainMenuScreen : Screen
    {
        private Container userInformation;
        private Context database;
        private Storage store;
        private APIController api;
        private TextureStore textures;
        private FillFlowContainer<LocalProjectSummaryContainer> projectsList;
        private FillFlowContainer<OnlineProjectSummaryContainer> onlineProjectsList;

        [BackgroundDependencyLoader]
        private void load(Context database, Storage store, APIController api, SplashInfoOverlay infoOverlay, TextureStore textures)
        {
            this.database = database;
            this.store = store;
            this.api = api;
            this.textures = textures;

            RelativePositionAxes = Axes.X;
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Colour4 (106,100,104, 255)      //Color fondo general
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    ColumnDimensions = new Dimension[]
                    {
                        new Dimension(GridSizeMode.Relative, 0.25f),
                        new Dimension(GridSizeMode.Distributed)
                    },
                    Content = new []
                    {
                        new Drawable[]
                        {
                            userInformation = new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                RelativePositionAxes = Axes.Both,
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = new Colour4 (145,144,144, 255)   //Color userInformation
                                    },
                                    new UserImageChangerButton
                                    {
                                        Position = new Vector2(0, 125),
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        ButtonSize = new Vector2(250)
                                    },
                                    new SpriteText
                                    {
                                        Text = api.LocalUser.Value.Username,
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        Position = new Vector2(0,450)
                                    },
                                    new GamesToGoButton
                                    {
                                        Text = "Perfil",
                                        BackgroundColour = new Colour4 (106,100,104, 255),  //Color Boton userInformation
                                        BorderColour = Colour4.Black,
                                        BorderThickness = 2f,
                                        RelativeSizeAxes = Axes.X,
                                        Masking = true,
                                        Height = 40,
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        Position = new Vector2(0,600),
                                        Action = showProfile
                                    },
                                    new GamesToGoButton
                                    {
                                        Text = "Cerrar Sesión",
                                        BackgroundColour = new Colour4 (106,100,104, 255),   //Color Boton userInformation
                                        BorderColour = Colour4.Black,
                                        BorderThickness = 2f,
                                        RelativeSizeAxes = Axes.X,
                                        Masking = true,
                                        Height = 40,
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        Position = new Vector2(0,700),
                                        Action = logout,
                                    }
                                }
                            },
                            new GridContainer
                            {
                                RelativeSizeAxes = Axes.Both,
                                RowDimensions = new Dimension[]
                                {
                                    new Dimension(),
                                    new Dimension(GridSizeMode.AutoSize),
                                },
                                Content = new []
                                {
                                    new Drawable[]
                                    {
                                        new BasicScrollContainer
                                        {
                                            Anchor = Anchor.TopCentre,
                                            Origin = Anchor.TopCentre,
                                            ClampExtension = 10,
                                            Padding = new MarginPadding() { Top = 200, Horizontal = 150 },
                                            RelativeSizeAxes = Axes.Both,
                                            Child = new FillFlowContainer
                                            {
                                                Anchor = Anchor.TopCentre,
                                                Origin = Anchor.TopCentre,
                                                Spacing = Vector2.Zero,
                                                RelativeSizeAxes = Axes.X,
                                                AutoSizeAxes = Axes.Y,
                                                Direction = FillDirection.Vertical,
                                                Children = new Drawable[]
                                                {
                                                    projectsList = new FillFlowContainer<LocalProjectSummaryContainer>
                                                    {
                                                        BorderColour = Colour4.Black,
                                                        BorderThickness = 3f,
                                                        Masking = true,
                                                        Anchor = Anchor.TopCentre,
                                                        Origin = Anchor.TopCentre,
                                                        Spacing = Vector2.Zero,
                                                        RelativeSizeAxes = Axes.X,
                                                        AutoSizeAxes = Axes.Y,
                                                        Direction = FillDirection.Vertical,
                                                    },
                                                    onlineProjectsList = new FillFlowContainer<OnlineProjectSummaryContainer>
                                                    {
                                                        BorderColour = Colour4.Black,
                                                        BorderThickness = 3f,
                                                        Masking = true,
                                                        Anchor = Anchor.TopCentre,
                                                        Origin = Anchor.TopCentre,
                                                        Spacing = Vector2.Zero,
                                                        RelativeSizeAxes = Axes.X,
                                                        AutoSizeAxes = Axes.Y,
                                                        Direction = FillDirection.Vertical,
                                                    }
                                                }
                                            }
                                        },
                                    },
                                    new Drawable[]
                                    {
                                        new GamesToGoButton
                                        {
                                            Text = "Crear Nuevo Proyecto",
                                            BackgroundColour = new Colour4 (145,144,144, 255),
                                            BorderColour = Colour4.Black,
                                            BorderThickness = 2f,
                                            RelativeSizeAxes = Axes.X,
                                            Masking = true,
                                            Height = 100,
                                            Anchor = Anchor.BottomCentre,
                                            Origin = Anchor.BottomCentre,
                                            Action = createProject
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            populateProjectList();
        }

        private void logout()
        {
            api.Logout();
            this.Exit();
        }

        private void showProfile()
        {
            LoadComponentAsync(new ProfileScreen(), ps => this.Push(ps));
        }

        public override void OnEntering(IScreen last)
        {
            base.OnEntering(last);

            this.MoveToX(-1).MoveToX(0, 1000, Easing.InOutQuart);
            userInformation.MoveToX(-1).Then().Delay(300).MoveToX(0, 1500, Easing.OutBounce);
        }

        public override bool OnExiting(IScreen next)
        {
            this.MoveToX(-1, 1000, Easing.InOutQuart);

            return base.OnExiting(next);
        }

        public override void OnResuming(IScreen last)
        {
            base.OnResuming(last);

            onlineProjectsList.Clear();
            projectsList.Clear();
            populateProjectList();
        }

        private void populateProjectList()
        {
            foreach (var proj in database.Projects)
            {
                projectsList.Add(new LocalProjectSummaryContainer(proj) { EditAction = OpenProject, DeleteAction = DeleteProject });
            }

            populateOnlineList();
        }

        private void populateOnlineList()
        {
            var getProjects = new GetAllProjectsRequest();
            getProjects.Success += u =>
            {
                foreach (var proj in u.Where(u => (!database.Projects.Any(dbp => dbp.OnlineProjectID == u.Id)) && (!onlineProjectsList.Children.Any(opli => opli.ID == u.Id))))
                {
                    onlineProjectsList.Add(new OnlineProjectSummaryContainer(proj) { ImportAction = importProject });
                }
            };
            api.Queue(getProjects);
        }

        public void OpenProject(WorkingProject project)
        {
            LoadComponentAsync(new ProjectEditor(project), pe => this.Push(pe));
        }

        public void DeleteProject(ProjectInfo project)
        {
            if (project.Relations != null)
                database.Relations.RemoveRange(project.Relations);
            projectsList.Remove(projectsList.Children.First(p => p.ProjectInfo.LocalProjectID == project.LocalProjectID));
            populateOnlineList();
            store.Delete($"files/{project.File.NewName}");
            int[] relationsToRemove = database.Relations.Where(fr => fr.Project.OnlineProjectID == project.LocalProjectID).Select(fr => fr.RelationID).ToArray();
            project.ImageRelation = null;
            database.SaveChanges();
            database.Relations.RemoveRange(project.Relations.AsEnumerable());
            database.Files.Remove(project.File);
            database.SaveChanges();
        }

        private void importProject(OnlineProject onlineProject)
        {
            string filename = store.GetFullPath(Path.Combine(@"download", @$"{onlineProject.Hash}.zip"));

            ProjectInfo futureInfo = new ProjectInfo
            {
                File = new Database.Models.File { NewName = Path.GetFileNameWithoutExtension(filename), Type = "project" },
                CreatorID = onlineProject.CreatorId,
                ComunityStatus = CommunityStatus.Clouded,
                LastEdited = onlineProject.DateTimeLastEdited,
                MaxNumberPlayers = onlineProject.Maxplayers,
                MinNumberPlayers = onlineProject.Minplayers,
                Name = onlineProject.Name,
                Description = onlineProject.Description,
                OnlineProjectID = onlineProject.Id,
            };
            using (var fileStream = store.GetStream(filename, FileAccess.Read, FileMode.Open))
            {
                using (ZipFile zip = ZipFile.Read(fileStream))
                {
                    foreach (ZipEntry e in zip)
                    {
                        e.Extract(store.GetFullPath("files"), ExtractExistingFileAction.DoNotOverwrite);
                        if (e.FileName != futureInfo.File.NewName)
                        {
                            var file = database.Files.FirstOrDefault(f => f.NewName == e.FileName);
                            if (file == null)
                                file = new Database.Models.File { NewName = e.FileName, Type = "image" };
                            futureInfo.Relations.Add(new FileRelation { Project = futureInfo, File = file });
                        }
                    }
                }
            }

            database.Add(futureInfo);
            database.SaveChanges();
            futureInfo.ImageRelation = futureInfo.Relations.FirstOrDefault(r => r.File.NewName == onlineProject.Image);
            var ret = WorkingProject.Parse(futureInfo, store, textures, api);
            database.SaveChanges();

            onlineProjectsList.Remove(onlineProjectsList.Children.First(o => o.ID == onlineProject.Id));
            projectsList.Add(new LocalProjectSummaryContainer(futureInfo) { EditAction = OpenProject, DeleteAction = DeleteProject });
        }

        private void createProject()
        {
            OpenProject(null);
        }
    }
}
