using System;
using GamesToGo.Desktop.Database.Models;
using GamesToGo.Desktop.Graphics;
using GamesToGo.Desktop.Project;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Screens
{
    public class ProjectEditor : Screen
    {
        private Screen currentScreen;

        private ScreenStack screenContainer;

        private readonly Bindable<IProjectElement> currentEditingElement = new Bindable<IProjectElement>();

        public IBindable<IProjectElement> CurrentEditingElement => currentEditingElement;

        private DependencyContainer dependencies;

        private WorkingProject workingProject;
        private EditorTabChanger tabsBar;
        private readonly ProjectInfo info;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            return dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
        }

        public ProjectEditor(ProjectInfo project)
        {
            info = project;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures, Storage store)
        {
            workingProject = new WorkingProject(info, store, textures);

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

        public void SelectElement(IProjectElement element)
        {
            currentEditingElement.Value = element;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
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
