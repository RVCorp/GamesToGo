using GamesToGo.Desktop.Project;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Screens
{
    public class ProjectEditor : Screen
    {
        private Screen currentScreen;

        private readonly ScreenStack screenContainer;

        private readonly Bindable<IProjectElement> currentEditingElement = new Bindable<IProjectElement>();

        private DependencyContainer dependencies;

        private readonly WorkingProject workingProject;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            return dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
        }

        public ProjectEditor(ProjectInfo project)
        {
            workingProject = new WorkingProject(project);

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
                        new BasicButton
                        {
                            RelativeSizeAxes = Axes.Y,
                            Width = 70,
                            Text = "Archivo",
                            BackgroundColour = Color4.Red,
                            Action = () => changeEditorScreen(EditorScreenOption.Archivo),
                        },
                        new BasicButton
                        {
                            Position = new Vector2(70,0),
                            RelativeSizeAxes = Axes.Y,
                            Width = 70,
                            Text = "Inicio",
                            BackgroundColour = Color4.DimGray,
                            Action = () => changeEditorScreen(EditorScreenOption.Inicio),
                        },
                        new BasicButton
                        {
                            Position = new Vector2(140,0),
                            RelativeSizeAxes = Axes.Y,
                            Width = 70,
                            Text = "Objetos",
                            BackgroundColour = Color4.DimGray,
                            Action = () => changeEditorScreen(EditorScreenOption.Objetos),
                        }
                    },
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            currentEditingElement.ValueChanged += _ => changeEditorScreen(EditorScreenOption.Objetos);

            dependencies.Cache(currentEditingElement);
            dependencies.Cache(workingProject);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            changeEditorScreen(EditorScreenOption.Inicio);
        }

        private void changeEditorScreen(EditorScreenOption option)
        {
            Screen tempScreen = null;
            switch (option)
            {
                case EditorScreenOption.Archivo:
                    if (!(currentScreen is ProjectFileScreen))
                        tempScreen = new ProjectFileScreen();
                    break;
                case EditorScreenOption.Inicio:
                    if (!(currentScreen is ProjectHomeScreen))
                        tempScreen = new ProjectHomeScreen();
                    break;
                case EditorScreenOption.Objetos:
                    if(!(currentScreen is ProjectObjectScreen))
                        tempScreen = new ProjectObjectScreen();
                    break;
                default:
                    break;
            }

            if(tempScreen != null)
            {
                currentScreen?.Exit();
                currentScreen = tempScreen;
                LoadComponentAsync(currentScreen, screenContainer.Push);
            }
        }

        private enum EditorScreenOption
        {
            Archivo,
            Inicio,
            Objetos,
        }
    }
}
