using GamesToGo.Desktop.Overlays;
using GamesToGo.Desktop.Graphics;
using System.Linq;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Project.Elements;
using Microsoft.EntityFrameworkCore.Internal;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Screens
{
    class ProjectEventsScreen : Screen
    {
        private IBindable<ProjectElement> currentEditing = new Bindable<ProjectElement>();
        private ProjectEditor editor;
        private WorkingProject project;
        private Container editEventAreaContainer;
        private Container noSelectionContainer;
        private FillFlowContainer EventGraphicsContainer;
        private FillFlowContainer<ProjectObjectEventContainer> eventsContainer;
        private EventCreationOverlay eventEditOverlay;
        private DependencyContainer dependencies;

        protected override IReadOnlyDependencyContainer CreateChildDependencies(IReadOnlyDependencyContainer parent)
        {
            return dependencies = new DependencyContainer(base.CreateChildDependencies(parent));
        }

        [BackgroundDependencyLoader]
        private void load(ProjectEditor editor, WorkingProject project)
        {
            this.editor = editor;
            this.project = project;
            dependencies.Cache(eventEditOverlay = new EventCreationOverlay());
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Color4 (106, 100, 104, 255),
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Content = new[]
                    {
                        new Drawable[]
                        {
                            //Listas de elementos
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable []
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = Color4.Gray
                                    },
                                    new ProjectObjectManagerContainer<Card>("Cartas", true)
                                    {
                                        Anchor = Anchor.TopLeft,
                                        Origin = Anchor.TopLeft,
                                        Height = 1/3f,
                                    },
                                    new ProjectObjectManagerContainer<Token>("Fichas", true)
                                    {
                                        Anchor = Anchor.CentreLeft,
                                        Origin = Anchor.CentreLeft,
                                        Height = 1/3f,
                                    },
                                    new ProjectObjectManagerContainer<Board>("Tableros", true)
                                    {
                                        Anchor = Anchor.BottomLeft,
                                        Origin = Anchor.BottomLeft,
                                        Height = 1/3f,
                                    }
                                }
                            },
                            editEventAreaContainer = new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable[]
                                {
                                    EventGraphicsContainer = new FillFlowContainer
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Direction = FillDirection.Vertical,
                                        Children = new Drawable[]
                                        {
                                            new Container
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                Children = new Drawable[]
                                                {
                                                    new Box
                                                    {
                                                        RelativeSizeAxes = Axes.Both,
                                                        Colour = Color4.Cyan
                                                    },
                                                    new Container
                                                    {
                                                        RelativeSizeAxes = Axes.Both,
                                                        Padding = new MarginPadding() { Horizontal = 60, Vertical = 50 },
                                                        Children = new Drawable[]
                                                        {
                                                            new Container
                                                            {
                                                                RelativeSizeAxes = Axes.Both,
                                                                Masking = true,
                                                                BorderThickness = 3f,
                                                                Children = new Drawable[]
                                                                {
                                                                    new Box
                                                                    {
                                                                        RelativeSizeAxes = Axes.Both,
                                                                        Colour = Color4.DarkGoldenrod
                                                                    },
                                                                    new GridContainer
                                                                    {
                                                                        RelativeSizeAxes = Axes.Both,
                                                                        RowDimensions = new Dimension[]
                                                                        {
                                                                            new Dimension(),
                                                                            new Dimension(GridSizeMode.AutoSize)
                                                                        },
                                                                        Content = new Drawable[][]
                                                                        {
                                                                            new Drawable[]
                                                                            {
                                                                                new BasicScrollContainer
                                                                                {
                                                                                    ClampExtension = 10,
                                                                                    RelativeSizeAxes = Axes.Both,
                                                                                    Child = eventsContainer = new FillFlowContainer<ProjectObjectEventContainer>
                                                                                    {
                                                                                        Spacing = Vector2.Zero,
                                                                                        RelativeSizeAxes = Axes.X,
                                                                                        AutoSizeAxes = Axes.Y,
                                                                                        Direction = FillDirection.Vertical,
                                                                                    },
                                                                                }
                                                                            },
                                                                            new Drawable[]
                                                                            {
                                                                                new Container
                                                                                {
                                                                                    Anchor = Anchor.BottomCentre,
                                                                                    Origin = Anchor.BottomCentre,
                                                                                    RelativeSizeAxes = Axes.X,
                                                                                    Height = 30,
                                                                                    Children = new Drawable[]
                                                                                    {
                                                                                        new Box
                                                                                        {
                                                                                            RelativeSizeAxes = Axes.Both,
                                                                                            Colour = Color4.Honeydew
                                                                                        },
                                                                                        new Container
                                                                                        {
                                                                                            Anchor = Anchor.BottomRight,
                                                                                            Origin = Anchor.BottomRight,
                                                                                            RelativeSizeAxes = Axes.Y,
                                                                                            Width = 100,
                                                                                            Child = new GamesToGoButton
                                                                                            {
                                                                                                RelativeSizeAxes = Axes.Both,
                                                                                                Text = "Añadir evento",
                                                                                                Action = eventOverlay
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            },
                                        }
                                    },
                                    noSelectionContainer = new Container
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        Child = new SpriteText
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Text = "Selecciona un objeto para editarlo",
                                        },
                                    },
                                    eventEditOverlay,
                                }
                            }
                        }
                    },
                    ColumnDimensions = new Dimension[]
                    {
                        new Dimension(GridSizeMode.Relative, 0.25f),
                        new Dimension(GridSizeMode.Distributed)
                    }
                }
            };
            currentEditing.BindTo(editor.CurrentEditingElement);
            currentEditing.BindValueChanged(checkData, true);
        }

        private void eventOverlay()
        {
            eventEditOverlay.Show();
        }

        private void checkData(ValueChangedEvent<ProjectElement> obj)
        {
            EventGraphicsContainer.FadeTo(obj.NewValue == null ? 0 : 1);
            noSelectionContainer.FadeTo(obj.NewValue == null ? 1 : 0);
        }
    }
}
