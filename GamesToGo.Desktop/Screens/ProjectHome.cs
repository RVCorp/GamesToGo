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
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = Color4.Yellow
                                    },
                                    new Container
                                    {
                                        Anchor = Anchor.BottomRight,
                                        Origin = Anchor.BottomRight,
                                        RelativeSizeAxes = Axes.X,
                                        Height = 50,
                                        Children = new Drawable[]
                                        {
                                            new Box
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                Colour = Color4.Beige
                                            },
                                            new BasicButton
                                            {
                                                RelativeSizeAxes = Axes.Y,
                                                Anchor = Anchor.BottomRight,
                                                Origin = Anchor.BottomRight,
                                                Width = 70,
                                                BackgroundColour = Color4.Red,
                                                BorderColour = Color4.Black,
                                                BorderThickness = 2.5f,
                                                Masking = true,
                                                Child =  new SpriteIcon
                                                {
                                                    Icon = FontAwesome.Solid.Ad,
                                                    Colour = Color4.Black,
                                                    RelativeSizeAxes = Axes.Y,
                                                    Width = 40,
                                                    Anchor = Anchor.Centre,
                                                    Origin = Anchor.Centre,
                                                },
                                                Action = () => allCards.Add(new Container
                                                {
                                                    Width = 158.4f,
                                                    Height = 250,
                                                    Children = new Drawable[]
                                                    {
                                                        new Box
                                                        {
                                                            Anchor = Anchor.Centre,
                                                            Origin = Anchor.Centre,
                                                            RelativeSizeAxes = Axes.Both,
                                                            Colour = Color4.Aquamarine
                                                        }
                                                    }
                                                })
                                            }
                                        }
                                    },
                                    allCards = new FillFlowContainer()
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Direction = FillDirection.Full,
                                        Children = new Drawable[]
                                        {
                                            
                                        }
                                    }
                                }
                            },
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = Color4.Blue
                                    },
                                    new Container
                                    {
                                        Anchor = Anchor.BottomRight,
                                        Origin = Anchor.BottomRight,
                                        RelativeSizeAxes = Axes.X,
                                        Height = 50,
                                        Children = new Drawable[]
                                        {
                                            new Box
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                Colour = Color4.Beige
                                            },
                                            new BasicButton
                                            {
                                                RelativeSizeAxes = Axes.Y,
                                                Anchor = Anchor.BottomRight,
                                                Origin = Anchor.BottomRight,
                                                Width = 70,
                                                BackgroundColour = Color4.Red,
                                                BorderColour = Color4.Black,
                                                BorderThickness = 2.5f,
                                                Masking = true,
                                                Child =  new SpriteIcon
                                                {
                                                    Icon = FontAwesome.Solid.Ad,
                                                    Colour = Color4.Black,
                                                    RelativeSizeAxes = Axes.Y,
                                                    Width = 40,
                                                    Anchor = Anchor.Centre,
                                                    Origin = Anchor.Centre,
                                                },
                                                Action = () => allTokens.Add(new Container
                                                {
                                                    Width = 158.4f,
                                                    Height = 250,
                                                    Children = new Drawable[]
                                                    {
                                                        new Box
                                                        {
                                                            Anchor = Anchor.Centre,
                                                            Origin = Anchor.Centre,
                                                            RelativeSizeAxes = Axes.Both,
                                                            Colour = Color4.Aquamarine
                                                        }
                                                    }
                                                })
                                            }
                                        }
                                    },
                                    allTokens = new FillFlowContainer()
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Direction = FillDirection.Full,
                                        Children = new Drawable[]
                                        {

                                        }
                                    }
                                }
                            },
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = Color4.Red
                                    },
                                    new Container
                                    {
                                        Anchor = Anchor.BottomRight,
                                        Origin = Anchor.BottomRight,
                                        RelativeSizeAxes = Axes.X,
                                        Height = 50,
                                        Children = new Drawable[]
                                        {
                                            new Box
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                Colour = Color4.Beige
                                            },
                                            new BasicButton
                                            {
                                                RelativeSizeAxes = Axes.Y,
                                                Anchor = Anchor.BottomRight,
                                                Origin = Anchor.BottomRight,
                                                Width = 70,
                                                BackgroundColour = Color4.Red,
                                                BorderColour = Color4.Black,
                                                BorderThickness = 2.5f,
                                                Masking = true,
                                                Child =  new SpriteIcon
                                                {
                                                    Icon = FontAwesome.Solid.Ad,
                                                    Colour = Color4.Black,
                                                    RelativeSizeAxes = Axes.Y,
                                                    Width = 40,
                                                    Anchor = Anchor.Centre,
                                                    Origin = Anchor.Centre,
                                                },
                                                Action = () => allBoxes.Add(new Container
                                                {
                                                    Width = 163,
                                                    Height = 250,
                                                    Children = new Drawable[]
                                                    {
                                                        new Box
                                                        {
                                                            Anchor = Anchor.Centre,
                                                            Origin = Anchor.Centre,
                                                            RelativeSizeAxes = Axes.Both,
                                                            Colour = Color4.Aquamarine
                                                        }
                                                    }
                                                })
                                            }
                                        }
                                    },
                                    allBoxes = new FillFlowContainer()
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Direction = FillDirection.Full,
                                        Children = new Drawable[]
                                        {

                                        }
                                    }
                                }
                            }
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
