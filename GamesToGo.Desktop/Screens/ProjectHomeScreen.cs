using System;
using GamesToGo.Desktop.Graphics;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Project.Elements;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
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
        private WorkingProject project;
        private NumericTextbox maxPlayersTextBox;
        private NumericTextbox minPlayersTextBox;
        private BasicTextBox descriptionTextBox;

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
                    ColumnDimensions = new Dimension[]
                    {
                        new Dimension(),
                    },
                    RowDimensions = new Dimension[]
                    {
                        new Dimension(GridSizeMode.AutoSize),
                        new Dimension()
                    },
                    Content = new Drawable[][]
                    {
                        new Drawable[]
                        {
                            new Container
                            {
                                AutoSizeAxes = Axes.Y,
                                RelativeSizeAxes = Axes.X,
                                Children = new Drawable[]
                                {
                                    new Container
                                    {
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        RelativeSizeAxes = Axes.X,
                                        Height = 180,
                                        Children = new Drawable[]
                                        {
                                            new Box
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                Colour = Color4.Black.Opacity(0.8f),
                                            },
                                            new Container
                                            {
                                                Padding = new MarginPadding(15),
                                                Size = new Vector2(180),
                                                Child = new ImageChangerButton()
                                                {
                                                    RelativeSizeAxes = Axes.Both,
                                                }
                                            },
                                            new SpriteText
                                            {
                                                Text = "Nombre del proyecto:",
                                                Position = new Vector2(180, 17)
                                            },
                                            titleTextBox = new BasicTextBox
                                            {
                                                Text = project.DatabaseObject.Name,
                                                Position = new Vector2(340, 10),
                                                Height = 35,
                                                Width = 775,
                                            },
                                            new SpriteText
                                            {
                                                Anchor = Anchor.TopCentre,
                                                Text = "Minimo Jugadores:",
                                                Position = new Vector2(560, 17)
                                            },
                                            minPlayersTextBox = new NumericTextbox(2)        //Restringir la cantidad de digitos a 2 
                                            {
                                                Text = Math.Max(2, project.DatabaseObject.MinNumberPlayers).ToString(),
                                                Anchor = Anchor.TopCentre,
                                                Position = new Vector2(694, 10),
                                                Height = 35,
                                                Width = 50,
                                                CommitOnFocusLost = true,
                                            },
                                            new SpriteText
                                            {
                                                Anchor = Anchor.TopCentre,
                                                Text = "Maximo Jugadores:",
                                                Position = new Vector2(760, 17)
                                            },
                                            maxPlayersTextBox = new NumericTextbox(2)        //Restringir la cantidad de digitos a 2 
                                            {
                                                Text = Math.Min(32, project.DatabaseObject.MaxNumberPlayers).ToString(),
                                                Anchor = Anchor.TopCentre,
                                                Position = new Vector2(898, 10),
                                                Height = 35,
                                                Width = 50,
                                                CommitOnFocusLost = true,
                                            },
                                            new SpriteText
                                            {
                                                Text = "Descripción:",
                                                Position = new Vector2(245,70)
                                            },
                                            descriptionTextBox = new BasicTextBox        //Textbox de varios renglones
                                            {
                                                Text = project.DatabaseObject.Description,
                                                Position = new Vector2(340,70),
                                                Height = 35,
                                                Width = 1732
                                            }
                                        }
                                    },
                                }
                            }
                        },
                        new Drawable[]
                        {
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Width = 0.5f,
                                Children = new Drawable[]
                                {
                                    new ProjectObjectManagerContainer<Card>("Cartas")
                                    {
                                        Anchor = Anchor.BottomLeft,
                                        Origin = Anchor.BottomLeft,
                                        Width = 1/3f,
                                    },
                                    new ProjectObjectManagerContainer<Token>("Fichas")
                                    {
                                        Anchor = Anchor.BottomCentre,
                                        Origin = Anchor.BottomCentre,
                                        Width = 1/3f,
                                    },
                                    new ProjectObjectManagerContainer<Board>("Tableros")
                                    {
                                        Anchor = Anchor.BottomRight,
                                        Origin = Anchor.BottomRight,
                                        Width = 1/3f,
                                    },
                                }
                            }
                        }
                    },
                }
            };

            descriptionTextBox.Current.ValueChanged += (obj) => project.DatabaseObject.Description = obj.NewValue;
            titleTextBox.Current.ValueChanged += (obj) => project.DatabaseObject.Name = obj.NewValue;
            maxPlayersTextBox.OnCommit += (_, __) => checkPlayerNumber(false);
            minPlayersTextBox.OnCommit += (_, __) => checkPlayerNumber(true);

            checkPlayerNumber(false);
        }

        private void checkPlayerNumber(bool isMin)
        {
            int minPlayers = Math.Clamp(int.Parse(minPlayersTextBox.Current.Value), 2, 32);
            int maxPlayers = Math.Clamp(int.Parse(maxPlayersTextBox.Current.Value), 2, 32);

            if (minPlayers > maxPlayers)
            {
                if (isMin)
                    maxPlayers = minPlayers;
                else
                    minPlayers = maxPlayers;
            }

            minPlayersTextBox.Text = minPlayers.ToString();
            project.DatabaseObject.MinNumberPlayers = minPlayers;

            maxPlayersTextBox.Text = maxPlayers.ToString();
            project.DatabaseObject.MaxNumberPlayers = maxPlayers;
        }
    }
}
