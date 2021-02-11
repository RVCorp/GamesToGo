using System;
using System.Linq;
using GamesToGo.Editor.Graphics;
using GamesToGo.Editor.Overlays;
using GamesToGo.Editor.Project;
using GamesToGo.Editor.Project.Elements;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osuTK;

namespace GamesToGo.Editor.Screens
{
    public class ProjectHomeScreen : Screen
    {
        public const float TEXT_ELEMENT_SIZE = 35f;

        private BasicTextBox titleTextBox;

        [Cached]
        private ArgumentTypeListing argumentListing = new ArgumentTypeListing();
        [Cached]
        private ArgumentSelectorOverlay selectorOverlay = new ArgumentSelectorOverlay();
        [Resolved]
        private WorkingProject project { get; set; }
        private NumericTextBox maxPlayersTextBox;
        private NumericTextBox minPlayersTextBox;
        private BasicTextBox descriptionTextBox;
        private BasicDropdown<ChatRecommendation> chatDropdown;
        private IteratingButton toggleButton;
        private SpriteText editingText;
        private TurnsOverlay turnsOverlay;
        private VictoryConditionsContainer victoryContainer;
        private PreparationTurnOverlay preparationTurnOverlay;
        private TagSelectionContainer tags;

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
                                                Height = TEXT_ELEMENT_SIZE,
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
                                                Height = TEXT_ELEMENT_SIZE,
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
                                                Height = TEXT_ELEMENT_SIZE,
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
                                                Height = TEXT_ELEMENT_SIZE,
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
                                            new SpriteText
                                            {
                                                Text = @"Etiquetas:",
                                                Position = new Vector2(556, 130),
                                            },
                                            tags = new TagSelectionContainer
                                            {
                                                Size = new Vector2(1272, TEXT_ELEMENT_SIZE),
                                                Position = new Vector2(635, 130),
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
                                    new Dimension(),
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
                                                            new FillFlowContainer
                                                            {
                                                                RelativeSizeAxes = Axes.Both,
                                                                Direction = FillDirection.Horizontal,
                                                                Children = new Drawable[]
                                                                {
                                                                    new Container
                                                                    {
                                                                        RelativeSizeAxes = Axes.Both,
                                                                        Width = .4f,
                                                                        Child = editingText = new SpriteText
                                                                        {
                                                                            Font = new FontUsage(size: 45),
                                                                            Text = @"Condiciones de Victoria  Turnos  Turno de preparación",
                                                                            Position = new Vector2(5, 2.5f),
                                                                        },
                                                                    },
                                                                    new FillFlowContainer
                                                                    {
                                                                        RelativeSizeAxes = Axes.Both,
                                                                        Width = .6f,
                                                                        Direction = FillDirection.Horizontal,
                                                                        Children = new Drawable[]
                                                                        {
                                                                            new Container
                                                                            {
                                                                                RelativeSizeAxes = Axes.Both,
                                                                                Children = new Drawable[]
                                                                                {
                                                                                    toggleButton = new IteratingButton
                                                                                    {
                                                                                        Anchor = Anchor.CentreRight,
                                                                                        Origin = Anchor.CentreRight,
                                                                                        Size = new Vector2(200, 35),
                                                                                        X = -5,
                                                                                        Text = @"Turnos",
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
                                                            preparationTurnOverlay = new PreparationTurnOverlay(),
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
                selectorOverlay,
            };
            toggleButton.Actions.Add(showVictoryConditions);
            toggleButton.Actions.Add(showTurns);
            toggleButton.Actions.Add(showPreparationTurn);
            chatDropdown.Current.Value = project.ChatRecommendation;
            chatDropdown.Current.BindValueChanged(cht => project.ChatRecommendation = cht.NewValue);
            descriptionTextBox.Current.BindValueChanged(obj => project.DatabaseObject.Description = obj.NewValue);
            titleTextBox.Current.BindValueChanged(obj => project.DatabaseObject.Name = obj.NewValue);
            maxPlayersTextBox.OnCommit += (_, __) => checkPlayerNumber(false);
            minPlayersTextBox.OnCommit += (_, __) => checkPlayerNumber(true);

            tags.Current.Value = project.DatabaseObject.Tags;
            tags.Current.BindValueChanged(tag => project.DatabaseObject.Tags = tag.NewValue);


            checkPlayerNumber(false);
        }

        private void showVictoryConditions()
        {
            victoryContainer.Show();
            turnsOverlay.Hide();
            preparationTurnOverlay.Hide();
            editingText.Text = @"Condiciones de Victoria";
            toggleButton.Text = @"Turnos";
        }

        private void showTurns()
        {
            victoryContainer.Hide();
            turnsOverlay.Show();
            preparationTurnOverlay.Hide();
            editingText.Text = @"Turnos";
            toggleButton.Text = @"Turno de Preparación";
        }

        private void showPreparationTurn()
        {
            victoryContainer.Hide();
            turnsOverlay.Hide();
            preparationTurnOverlay.Show();
            editingText.Text = @"Turno de Preparación";
            toggleButton.Text = @"Condiciones de Victoria";
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

            minPlayersTextBox.Current.Value = minPlayers;
            project.DatabaseObject.MinNumberPlayers = minPlayers;

            maxPlayersTextBox.Current.Value = maxPlayers;
            project.DatabaseObject.MaxNumberPlayers = maxPlayers;
        }
    }
}
