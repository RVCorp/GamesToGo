using System;
using System.Linq;
using GamesToGo.Desktop.Graphics;
using GamesToGo.Desktop.Overlays;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Project.Elements;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osuTK;

namespace GamesToGo.Desktop.Screens
{
    public class ProjectHomeScreen : Screen
    {
        private BasicTextBox titleTextBox;

        [Cached]
        private ArgumentTypeListing argumentListing = new ArgumentTypeListing();
        [Resolved]
        private WorkingProject project { get; set; }
        private NumericTextBox maxPlayersTextBox;
        private NumericTextBox minPlayersTextBox;
        private BasicTextBox descriptionTextBox;
        private BasicDropdown<ChatRecommendation> chatDropdown;
        private GamesToGoButton toggleButton;
        private SpriteText editingText;
        private TurnsOverlay turnsOverlay;
        private VictoryConditionsContainer victoryContainer;

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Colour4(106, 100, 104, 255), //Color fondo general
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    ColumnDimensions = new[]
                    {
                        new Dimension(),
                    },
                    RowDimensions = new[]
                    {
                        new Dimension(GridSizeMode.AutoSize),
                        new Dimension(),
                    },
                    Content = new[]
                    {
                        new Drawable[]
                        {
                            new Container
                            {
                                Depth = 0,
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
                                                Colour = Colour4.Black.Opacity(0.8f),
                                            },
                                            new Container
                                            {
                                                Padding = new MarginPadding(15),
                                                Size = new Vector2(180),
                                                Child = new ProjectImageChangerButton(),
                                            },
                                            new SpriteText
                                            {
                                                Text = @"Nombre del proyecto:",
                                                Position = new Vector2(180, 17),
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
                                                Text = @"Minimo Jugadores:",
                                                Position = new Vector2(560, 17),
                                            },
                                            minPlayersTextBox = new NumericTextBox(false)
                                            {
                                                LengthLimit = 2,
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
                                                Text = @"Maximo Jugadores:",
                                                Position = new Vector2(760, 17),
                                            },
                                            maxPlayersTextBox = new NumericTextBox(false)
                                            {
                                                LengthLimit = 2,
                                                Text = Math.Min(32, project.DatabaseObject.MaxNumberPlayers).ToString(),
                                                Anchor = Anchor.TopCentre,
                                                Position = new Vector2(898, 10),
                                                Height = 35,
                                                Width = 50,
                                                CommitOnFocusLost = true,
                                            },
                                            new SpriteText
                                            {
                                                Text = @"Descripción:",
                                                Position = new Vector2(245, 70),
                                            },
                                            descriptionTextBox = new BasicTextBox
                                            {
                                                Text = project.DatabaseObject.Description,
                                                Position = new Vector2(340, 70),
                                                Height = 35,
                                                Width = 1732,
                                            },
                                            new SpriteText
                                            {
                                                Origin = Anchor.TopRight,
                                                Text = @"Chat recomendado:",
                                                Position = new Vector2(329, 130),
                                            },
                                            chatDropdown = new GamesToGoDropdown<ChatRecommendation>
                                            {
                                                Width = 200,
                                                Position = new Vector2(340, 130),
                                                Items = Enum.GetValues(typeof(ChatRecommendation)).Cast<ChatRecommendation>(),
                                            },
                                        },
                                    },
                                },
                            },
                        },
                        new Drawable[]
                        {
                            new GridContainer
                            {
                                Depth = 1,
                                RelativeSizeAxes = Axes.Both,
                                ColumnDimensions = new[]
                                {
                                    new Dimension(GridSizeMode.Relative, 1/6f),
                                    new Dimension(GridSizeMode.Relative, 1/6f),
                                    new Dimension(GridSizeMode.Relative, 1/6f),
                                    new Dimension(),
                                },
                                RowDimensions = new[]
                                {
                                    new Dimension()
                                },
                                Content = new[]
                                {
                                    new Drawable[]
                                    {
                                        new ProjectObjectManagerContainer<Card>(),
                                        new ProjectObjectManagerContainer<Token>(),
                                        new ProjectObjectManagerContainer<Board>(),
                                        new GridContainer
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            RowDimensions = new[]
                                            {
                                                new Dimension(GridSizeMode.Absolute, 50),
                                                new Dimension(),
                                            },
                                            ColumnDimensions = new[]
                                            {
                                                new Dimension(),
                                            },
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
                                                                Colour = Colour4.MediumPurple,
                                                            },
                                                            editingText = new SpriteText
                                                            {
                                                                Font = new FontUsage(size: 45),
                                                                Position = new Vector2(5, 2.5f),
                                                            },
                                                            toggleButton = new GamesToGoButton
                                                            {
                                                                Anchor = Anchor.CentreRight,
                                                                Origin = Anchor.CentreRight,
                                                                Size = new Vector2(200, 35),
                                                                X = -5,
                                                                Action = () =>
                                                                {
                                                                    victoryContainer.State.Value = turnsOverlay.State.Value;
                                                                    turnsOverlay.ToggleVisibility();
                                                                },
                                                            },
                                                        },
                                                    },
                                                },
                                                new Drawable[]
                                                {
                                                    new Container
                                                    {
                                                        RelativeSizeAxes = Axes.Both,
                                                        Children = new Drawable[]
                                                        {
                                                            victoryContainer = new VictoryConditionsContainer
                                                            {
                                                                State = { Value = Visibility.Visible },
                                                            },
                                                            turnsOverlay = new TurnsOverlay(),
                                                        },
                                                    },
                                                },
                                            },
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
                argumentListing,
            };

            chatDropdown.Current.Value = project.ChatRecommendation;
            chatDropdown.Current.BindValueChanged(cht => project.ChatRecommendation = cht.NewValue);
            descriptionTextBox.Current.BindValueChanged(obj => project.DatabaseObject.Description = obj.NewValue);
            titleTextBox.Current.BindValueChanged(obj => project.DatabaseObject.Name = obj.NewValue);
            maxPlayersTextBox.OnCommit += (_, __) => checkPlayerNumber(false);
            minPlayersTextBox.OnCommit += (_, __) => checkPlayerNumber(true);
            turnsOverlay.State.BindValueChanged(_ => toggleEdition(), true);


            checkPlayerNumber(false);
        }

        private void toggleEdition()
        {
            switch (turnsOverlay.State.Value)
            {
                case Visibility.Hidden:
                    toggleButton.Text = @"Sistema de Turnos";
                    editingText.Text = @"Condiciones de Victoria";
                    break;
                case Visibility.Visible:
                    editingText.Text = @"Turnos";
                    toggleButton.Text = @"Condiciones de Victoria";
                    break;
            }
        }

        private void checkPlayerNumber(bool isMin)
        {
            int minPlayers = Math.Clamp((int)minPlayersTextBox.Current.Value, 2, 32);
            int maxPlayers = Math.Clamp((int)maxPlayersTextBox.Current.Value, 2, 32);

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
