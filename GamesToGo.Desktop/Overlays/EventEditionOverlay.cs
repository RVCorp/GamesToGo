using GamesToGo.Desktop.Graphics;
using GamesToGo.Desktop.Project.Actions;
using GamesToGo.Desktop.Project.Events;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Overlays
{
    public class EventEditionOverlay : OverlayContainer, IHasCurrentValue<ProjectEvent>
    {
        private FillFlowContainer<ActionDescriptor> actionFillFlow;
        private Box shadowBox;
        private Container contentContainer;
        private BasicTextBox eventNameBox;
        private Container eventDescriptorContainer;

        public Bindable<ProjectEvent> Current { get; set; } = new Bindable<ProjectEvent>();

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;

            Children = new Drawable[]
            {
                shadowBox = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0,
                },
                contentContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding(20),
                    Child = new GridContainer
                    {
                        RelativeSizeAxes = Axes.Both,
                        RowDimensions = new[]
                        {
                            new Dimension(GridSizeMode.AutoSize),
                            new Dimension(GridSizeMode.AutoSize),
                            new Dimension(),
                            new Dimension(GridSizeMode.Absolute, 30),
                        },
                        Content = new[]
                        {
                            new Drawable[]
                            {
                                new FillFlowContainer
                                {
                                    Name = @"Nombre evento",
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Direction = FillDirection.Horizontal,
                                    Padding = new MarginPadding {Bottom = 5},
                                    Spacing = new Vector2(7),
                                    Children = new Drawable[]
                                    {
                                        new SpriteText
                                        {
                                            Anchor = Anchor.BottomLeft,
                                            Origin = Anchor.BottomLeft,
                                            Text = @"Nombre del evento:",
                                            Font = new FontUsage(size: 20),
                                        },
                                        eventNameBox = new BasicTextBox
                                        {
                                            Anchor = Anchor.BottomLeft,
                                            Origin = Anchor.BottomLeft,
                                            Size = new Vector2(400, 30),
                                        },
                                    },
                                },
                            },
                            new Drawable[]
                            {
                                eventDescriptorContainer = new Container
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                },
                            },
                            new Drawable[]
                            {
                                new BasicScrollContainer
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    ClampExtension = 30,
                                    Child = actionFillFlow = new FillFlowContainer<ActionDescriptor>
                                    {
                                        AutoSizeAxes = Axes.Y,
                                        RelativeSizeAxes = Axes.X,
                                        Direction = FillDirection.Vertical,
                                    },
                                },
                            },
                            new Drawable[]
                            {
                                new Container
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Child = new ActionTypeListing
                                    {
                                        Anchor = Anchor.BottomCentre,
                                        Origin = Anchor.BottomCentre,
                                    },
                                },
                            },
                        },
                    },
                },
            };
        }

        public void ShowEvent(ProjectEvent model)
        {
            Current.Value?.Name.UnbindAll();

            Current.Value = model;
            eventNameBox.Text = model.Name.Value;
            Current.Value.Name.BindTo(eventNameBox.Current);

            Current.Value.Actions.CollectionChanged += (_, __) => recreateActions();

            eventDescriptorContainer.Child = new EventDescriptor(model);

            recreateActions();

            Show();
        }

        private void recreateActions()
        {
            actionFillFlow.Clear();
            foreach (var action in Current.Value.Actions)
            {
                actionFillFlow.Add(new ActionDescriptor(action));
            }
        }

        public void AddActionToCurrent(EventAction action)
        {
            Current.Value.Actions.Add(action);
        }

        protected override void PopIn()
        {
            shadowBox.FadeTo(0.5f, 250);
            contentContainer.FadeIn();
        }

        protected override void PopOut()
        {
            shadowBox.FadeOut(250, Easing.OutExpo);
            contentContainer.FadeOut();
        }
    }
}
