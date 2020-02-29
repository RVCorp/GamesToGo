using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osuTK.Graphics;
using osuTK;
using GamesToGo.Desktop.Graphics;

namespace GamesToGo.Desktop.Screens
{
    /// <summary>
    /// Pantalla del menu principal, muestra los proyectos del usuario, su perfil, y un modal para cerrar sesión. (WIP)
    /// </summary>
    public class MainMenuScreen : Screen
    {
        private Box background;
        private GridContainer screenDivition;
        private Container userInformation;
        private Container proyectCreation;

        public MainMenuScreen()
        {
            InternalChildren = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Color4 (106,100,104, 255)      //Color fondo general
                },

                screenDivition = new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Content = new []
                    {
                        new Drawable[]
                        {
                            userInformation =new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour= new Color4 (145,144,144, 255)   //Color userInformation
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
                                        Position= new Vector2(0,125),
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
                                        Text= "Perfil",
                                        BackgroundColour = background.Colour,   //Color Boton userInformation
                                        BorderColour = Color4.Black,
                                        BorderThickness = 2f,
                                        RelativeSizeAxes = Axes.X,
                                        Masking=true,
                                        Height = 40,
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        Position = new Vector2(0,600)
                                    },
                                    new BasicButton
                                    {
                                        Text= "Cerrar Sesión",
                                        BackgroundColour = background.Colour,   //Color Boton userInformation
                                        BorderColour = Color4.Black,
                                        BorderThickness = 2f,
                                        RelativeSizeAxes = Axes.X,
                                        Masking=true,
                                        Height = 40,
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        Position = new Vector2(0,700)
                                    }
                                }
                            },
                            proyectCreation = new Container
                            {
                                RelativeSizeAxes =Axes.Both,
                                Children = new Drawable[]
                                {
                                    new BasicButton
                                    {
                                        Text= "Crear Nuevo Proyecto",
                                        BackgroundColour = new Color4 (145,144,144, 255),   //
                                        BorderColour = Color4.Black,
                                        BorderThickness = 2f,
                                        RelativeSizeAxes = Axes.X,
                                        Masking = true,
                                        Height = 100,
                                        Anchor = Anchor.BottomCentre,
                                        Origin = Anchor.BottomCentre,
                                    },
                                    new FillFlowContainer
                                    {
                                        BorderColour = Color4.Black,
                                        BorderThickness = 3f,
                                        Masking = true,
                                        Height = 650,
                                        Width = 1000,
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        Position = new Vector2(0,200),
                                        Children = new Drawable[]
                                        {
                                            new ProjectDescriptionButton
                                            {
                                                RelativeSizeAxes= Axes.X,
                                                Height = 30
                                            },
                                            new ProjectDescriptionButton
                                            {
                                                RelativeSizeAxes= Axes.X,
                                                Height = 30
                                            },
                                            new ProjectDescriptionButton
                                            {
                                                RelativeSizeAxes= Axes.X,
                                                Height = 30
                                            },
                                            new ProjectDescriptionButton
                                            {
                                                RelativeSizeAxes= Axes.X,
                                                Height = 30
                                            },
                                            new ProjectDescriptionButton
                                            {
                                                RelativeSizeAxes= Axes.X,
                                                Height = 30
                                            },
                                            new ProjectDescriptionButton
                                            {
                                                RelativeSizeAxes= Axes.X,
                                                Height = 30
                                            },
                                            new ProjectDescriptionButton
                                            {
                                                RelativeSizeAxes= Axes.X,
                                                Height = 30
                                            },
                                            new ProjectDescriptionButton
                                            {
                                                RelativeSizeAxes= Axes.X,
                                                Height = 30
                                            },
                                            new ProjectDescriptionButton
                                            {
                                                RelativeSizeAxes= Axes.X,
                                                Height = 30
                                            },
                                            new ProjectDescriptionButton
                                            {
                                                RelativeSizeAxes= Axes.X,
                                                Height = 30
                                            },
                                        }
                                    }
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
        }
        protected override void LoadComplete()
        {
            userInformation.MoveToX(-1).Then().MoveToX(0, 10000, Easing.OutBounce);
        }
    }
}
