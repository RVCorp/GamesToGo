using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osuTK.Graphics;
using osuTK;
using GamesToGo.Desktop.Graphics;
using GamesToGo.Desktop.Database.Models;
using GamesToGo.Desktop.Project;
using System.Linq;
using osu.Framework.Platform;

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
        private FillFlowContainer<ProjectSummaryContainer> projectsList;

        [BackgroundDependencyLoader]
        private void load(Context database, Storage store)
        {
            this.database = database;
            this.store = store;
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Color4 (106,100,104, 255)      //Color fondo general
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
                                        Colour = new Color4 (145,144,144, 255)   //Color userInformation
                                    },
                                    new CircularContainer
                                    {
                                        Size = new Vector2(250),
                                        Child = new Box         //Cambiar Box por Sprite
                                        {
                                            RelativeSizeAxes = Axes.Both
                                            //FillMode= FillMode.Fill,
                                        },
                                        BorderColour = Color4.Black,
                                        BorderThickness = 3.5f,
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        Position = new Vector2(0,125),
                                        Masking = true
                                    },
                                    new SpriteText
                                    {
                                        Text = "StUpIdUsErNaMe27",
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        Position = new Vector2(0,450)
                                    },
                                    new BasicButton
                                    {
                                        Text = "Perfil",
                                        BackgroundColour = new Color4 (106,100,104, 255),  //Color Boton userInformation
                                        BorderColour = Color4.Black,
                                        BorderThickness = 2f,
                                        RelativeSizeAxes = Axes.X,
                                        Masking = true,
                                        Height = 40,
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        Position = new Vector2(0,600)
                                    },
                                    new BasicButton
                                    {
                                        Text = "Cerrar Sesión",
                                        BackgroundColour = new Color4 (106,100,104, 255),   //Color Boton userInformation
                                        BorderColour = Color4.Black,
                                        BorderThickness = 2f,
                                        RelativeSizeAxes = Axes.X,
                                        Masking = true,
                                        Height = 40,
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        Position = new Vector2(0,700),
                                        Action = () => this.Exit(),
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
                                            Child = projectsList = new FillFlowContainer<ProjectSummaryContainer>
                                            {
                                                BorderColour = Color4.Black,
                                                BorderThickness = 3f,
                                                Masking = true,
                                                Anchor = Anchor.TopCentre,
                                                Origin = Anchor.TopCentre,
                                                Spacing = new Vector2(0, 7),
                                                RelativeSizeAxes = Axes.X,
                                                AutoSizeAxes = Axes.Y,
                                                Direction = FillDirection.Vertical,
                                            },
                                        },
                                    },
                                    new Drawable[]
                                    {
                                        new BasicButton
                                        {
                                            Text = "Crear Nuevo Proyecto",
                                            BackgroundColour = new Color4 (145,144,144, 255),
                                            BorderColour = Color4.Black,
                                            BorderThickness = 2f,
                                            RelativeSizeAxes = Axes.X,
                                            Masking = true,
                                            Height = 100,
                                            Anchor = Anchor.BottomCentre,
                                            Origin = Anchor.BottomCentre,
                                            Action = createProject,
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

        protected override void LoadComplete()
        {
            userInformation.MoveToX(-1).Then().MoveToX(0, 1500, Easing.OutBounce);
        }

        public override void OnResuming(IScreen last)
        {
            base.OnResuming(last);

            projectsList.Clear();
            populateProjectList();
        }

        private void populateProjectList()
        {
            foreach (var proj in database.Projects)
            {
                projectsList.Add(new ProjectSummaryContainer(proj) { EditAction = OpenProject, DeleteAction = DeleteProject });
            }
        }

        public void OpenProject(WorkingProject project)
        {
            this.Push(new ProjectEditor(project));
        }
        public void DeleteProject(ProjectInfo project)
        {
            if (project.Relations != null)
                database.Relations.RemoveRange(project.Relations);
            projectsList.Remove(projectsList.Children.First(p => p.ProjectInfo.LocalProjectID == project.LocalProjectID));
            store.Delete($"files/{project.File.NewName}");
            database.Files.Remove(project.File);
            database.Projects.Remove(project);
            database.SaveChanges();
        }

        private void createProject()
        {
            OpenProject(null);
        }
    }
}
