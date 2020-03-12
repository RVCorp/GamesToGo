using System;
using System.Collections.Generic;
using System.Linq;
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
using GamesToGo.Desktop.Database.Models;
using GamesToGo.Desktop.Project;

namespace GamesToGo.Desktop.Screens
{
    /// <summary>
    /// Pantalla del menu principal, muestra los proyectos del usuario, su perfil, y un modal para cerrar sesión. (WIP)
    /// </summary>
    public class MainMenuScreen : Screen
    {
        private Container userInformation;
        private FillFlowContainer projectsList;

        public MainMenuScreen()
        {
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
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable[]
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
                                        Action = () => this.Push(new ProjectHome(new WorkingProject(new ProjectInfo())))
                                    },
                                    projectsList = new FillFlowContainer
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
                                            new ProjectDescriptionButton(new WorkingProject(new ProjectInfo()))
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

        [BackgroundDependencyLoader]
        private void load(Context database)
        {
            foreach(var project in database.Projects)
            {
                projectsList.Add(new ProjectDescriptionButton(new WorkingProject(project)));
            }
        }

        protected override void LoadComplete()
        {
            userInformation.MoveToX(-1).Then().MoveToX(0, 1500, Easing.OutBounce);
        }
    }
}
