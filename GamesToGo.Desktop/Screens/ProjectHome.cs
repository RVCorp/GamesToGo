using GamesToGo.Desktop.Graphics;
using GamesToGo.Desktop.Proyect;
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
    public class ProjectHome : Screen
    {
        private BasicTextBox titleTextBox;
        private FillFlowContainer allCards;
        private FillFlowContainer allTokens;
        private FillFlowContainer allBoxes;
        public ProjectHome(WorkingProject project)
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Color4 (106,100,104, 255)      //Color fondo general
                },
                new Container
                {
                    RelativeSizeAxes = Axes.X,
                    Height = 30,
                    Children= new Drawable[]
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
                            BackgroundColour = Color4.Red
                        },
                        new BasicButton
                        {
                            Position = new Vector2(70,0),
                            RelativeSizeAxes = Axes.Y,
                            Width = 70,
                            Text = "Inicio",
                            BackgroundColour = Color4.DimGray
                        },
                        new BasicButton
                        {
                            Position = new Vector2(140,0),
                            RelativeSizeAxes = Axes.Y,
                            Width = 70,
                            Text = "Objetos",
                            BackgroundColour = Color4.DimGray
                        }
                    }
                },
                new Container
                {
                    Anchor= Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Position = new Vector2(0,30),
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
                            Width = 775
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
                new GridContainer
                {
                    Position = new Vector2(0,210),
                    RelativeSizeAxes = Axes.X,
                    Height = 870,
                    Content = new[]
                    {
                        new Drawable[]
                        {
                            new ProjectObjectManagerContainer<IProjectElement>(),
                            new ProjectObjectManagerContainer<IProjectElement>(),
                            new ProjectObjectManagerContainer<IProjectElement>()
                        }
                    },
                    ColumnDimensions = new Dimension[]
                    {
                        new Dimension(GridSizeMode.Relative, 0.33f),
                        new Dimension(GridSizeMode.Relative, 0.33f),
                        new Dimension(GridSizeMode.Distributed)
                    }
                }
            };
        }
    }
}
