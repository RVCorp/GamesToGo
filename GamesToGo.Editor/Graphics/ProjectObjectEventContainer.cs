using GamesToGo.Editor.Overlays;
using GamesToGo.Editor.Project.Events;
using GamesToGo.Editor.Screens;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;

namespace GamesToGo.Editor.Graphics
{
    public class ProjectObjectEventContainer : Container
    {
        public readonly ProjectEvent Event;
        private SpriteText nameText;
        private SpriteText descriptionText;

        [Resolved]
        private EventEditionOverlay eventOverlay { get; set; }

        [Resolved]
        private ProjectEventsScreen eventsScreen { get; set; }

        public ProjectObjectEventContainer(ProjectEvent projectEvent)
        {
            Event = projectEvent;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.X;
            Height = 80;
            Masking = true;
            BorderThickness = 3f;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.SlateGray,
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
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable[]
                                {
                                    nameText = new SpriteText
                                    {
                                        Margin = new MarginPadding(2) { Left = 5 },
                                        Text = Event.Name.Value,
                                        Font = new FontUsage(size: 35),
                                    },
                                    descriptionText = new SpriteText
                                    {
                                        Anchor = Anchor.BottomLeft,
                                        Origin = Anchor.BottomLeft,
                                        Margin = new MarginPadding { Left = 5, Right = 2, Top = 2, Bottom = 7},
                                        Text = @$"{Event.Text}",
                                        Font = new FontUsage(size: 20),
                                    },
                                },
                            },
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Child = new IconButton(FontAwesome.Solid.Edit)
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Action = () => eventOverlay.ShowEvent(Event),
                                },
                            },
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Child = new IconButton(FontAwesome.Solid.TrashAlt)
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Action = () => eventsScreen.RemoveEvent(Event),
                                },
                            },
                        },
                    },
                    ColumnDimensions = new[]
                    {
                        new Dimension(GridSizeMode.Relative, 0.85f),
                        new Dimension(GridSizeMode.Relative, 0.075f),
                        new Dimension(),
                    },
                },
            };

            nameText.Current.BindTo(Event.Name);
            Event.Priority.BindValueChanged(_ => recreateText());
            Event.Actions.BindCollectionChanged((_, _) => recreateText());
            Event.Condition.BindValueChanged(_ => recreateText());
        }

        private void recreateText()
        {
            descriptionText.Text = @$"{Event.Actions.Count} acciones, Prioridad {Event.Priority}, {(Event.Condition.Value == null ? @"sin condición" : @"condicionado")}";
        }
    }
}
