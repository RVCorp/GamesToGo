using GamesToGo.Desktop.Graphics;
using GamesToGo.Desktop.Project;
using osu.Framework.Allocation;
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
    public class ProjectHomeScreen : Screen
    {
        private BasicTextBox titleTextBox;
        private ProjectObjectManagerContainer<IProjectElement> allCards;
        private ProjectObjectManagerContainer<IProjectElement> allTokens;
        private ProjectObjectManagerContainer<IProjectElement> allBoards;
        private WorkingProject project;

        [BackgroundDependencyLoader]
        private void load(WorkingProject project)
        {
            this.project = project;
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
                    Content = new[]
                    {
                        new Drawable[]
                        {
                            new Container
                            {
                                Anchor= Anchor.TopCentre,
                                Origin = Anchor.TopCentre,
                                RelativeSizeAxes = Axes.X,
                                Height = 180,
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = Color4.Black
                                    },
                                    new SpriteText
                                    {
                                        Text = "Nombre del proyecto:",
                                        Position = new Vector2(15,17)
                                    },
                                    titleTextBox = new BasicTextBox
                                    {
                                        Text = project.Title.Value,
                                        Position = new Vector2(175,10),
                                        Height = 35,
                                        Width = 775,
                                    },
                                    new SpriteText
                                    {
                                        Anchor = Anchor.TopCentre,
                                        Text = "Minimo Jugadores:",
                                        Position = new Vector2(560, 17)
                                    },
                                    new BasicTextBox        //Restringir la cantidad de digitos a 2 
                                    {
                                        Anchor = Anchor.TopCentre,
                                        Position = new Vector2(694, 10),
                                        Height = 35,
                                        Width = 50
                                    },
                                    new SpriteText
                                    {
                                        Anchor = Anchor.TopCentre,
                                        Text = "Maximo Jugadores:",
                                        Position = new Vector2(760, 17)
                                    },
                                    new BasicTextBox        //Restringir la cantidad de digitos a 2 
                                    {
                                        Anchor = Anchor.TopCentre,
                                        Position = new Vector2(898, 10),
                                        Height = 35,
                                        Width = 50
                                    },
                                    new SpriteText
                                    {
                                        Text = "Descripción:",
                                        Position = new Vector2(80,70)
                                    },
                                    new BasicTextBox        //Textbox de varios renglones
                                    {
                                        Position = new Vector2(175,70),
                                        Height = 100,
                                        Width = 1732
                                    }
                                }
                            },
                        },
                        new Drawable[]
                        {
                            new GridContainer
                            {
                                RelativeSizeAxes = Axes.Both,
                                Content = new[]
                                {
                                    new Drawable[]
                                    {
                                        allCards = new ProjectObjectManagerContainer<IProjectElement>(),
                                        allTokens = new ProjectObjectManagerContainer<IProjectElement>(),
                                        allBoards = new ProjectObjectManagerContainer<IProjectElement>()
                                    }
                                },
                                ColumnDimensions = new Dimension[]
                                {
                                    new Dimension(GridSizeMode.Distributed),
                                    new Dimension(GridSizeMode.Distributed),
                                    new Dimension(GridSizeMode.Distributed)
                                }
                            }
                        },
                    },
                    ColumnDimensions = new Dimension[]
                    {
                        new Dimension(GridSizeMode.Distributed)
                    },
                    RowDimensions = new Dimension[]
                    {
                        new Dimension(GridSizeMode.AutoSize),
                        new Dimension(GridSizeMode.Distributed)
                    }
                },
            };
        }
    }
}
