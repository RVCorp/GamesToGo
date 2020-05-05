using System;
using System.Collections.Generic;
using System.Text;
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
    public class ProjectObjectScreen : Screen
    {
        WorkingProject project;
        ElementListContainer allCards, allTokens, allBoards;
        [BackgroundDependencyLoader]
        private void load (WorkingProject project)
        {
            this.project = project;
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Color4 (106,100,104, 255)
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Content = new []
                    {
                        new Drawable[]
                        {
                            new Container
                            {
                                RelativeSizeAxes =Axes.Both,
                                Children = new Drawable []
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = Color4.Gray
                                    },
                                    allCards = new ElementListContainer(),
                                    allTokens = new ElementListContainer(),
                                    allBoards = new ElementListContainer()
                                }
                            },
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable[]
                                {
                                    new Container
                                    {
                                        RelativeSizeAxes = Axes.X,
                                        Height = 600,
                                        Children = new Drawable[]
                                        {
                                            new Box
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                Colour = Color4.Cyan
                                            },
                                            new Box //Imagen del objeto
                                            {
                                                Width = 500,
                                                Height = 500,
                                                Position = new Vector2(60,50),
                                                Colour = Color4.Black
                                            },
                                            new SpriteText
                                            {
                                                Anchor = Anchor.TopRight,
                                                Origin = Anchor.TopRight,
                                                Position = new Vector2(-675,60),
                                                Text = "Nombre:",
                                                Colour = Color4.Black
                                            },
                                            new BasicTextBox
                                            {
                                                Anchor = Anchor.TopRight,
                                                Origin = Anchor.TopRight,
                                                Position = new Vector2(-250,50),
                                                Height = 35,
                                                Width = 400
                                            },
                                            new SpriteText
                                            {
                                                Anchor = Anchor.TopRight,
                                                Origin = Anchor.TopRight,
                                                Position = new Vector2(-675,130),
                                                Text = "Descripcion:",
                                                Colour = Color4.Black
                                            },
                                            new BasicTextBox
                                            {
                                                Anchor = Anchor.TopRight,
                                                Origin = Anchor.TopRight,
                                                Position = new Vector2(-250, 120),
                                                Height = 200,
                                                Width = 400
                                            }
                                        }
                                    },
                                    new Container
                                    {
                                        RelativeSizeAxes = Axes.X,
                                        Height = 400,
                                        Anchor = Anchor.BottomRight,
                                        Origin = Anchor.BottomRight,
                                        Children = new Drawable[]
                                        {
                                            new Box
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                Colour = Color4.Fuchsia
                                            }
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
    }
}
