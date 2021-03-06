using System;
using GamesToGo.Editor.Graphics;
using GamesToGo.Editor.Project.Actions;
using GamesToGo.Editor.Project.Events;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;

namespace GamesToGo.Editor.Overlays
{
    public class EventEditionOverlay : OverlayContainer, IHasCurrentValue<ProjectEvent>
    {
        private FillFlowContainer<ActionDescriptor> actionFillFlow;
        private BasicTextBox eventNameBox;
        private Container eventDescriptorContainer;
        private NumericTextBox priorityBox;

        [Cached]
        private ArgumentTypeListing argumentListing = new ArgumentTypeListing();
        [Cached]
        private ArgumentSelectorOverlay selectorOverlay = new ArgumentSelectorOverlay();

        public Bindable<ProjectEvent> Current { get; set; } = new Bindable<ProjectEvent>();

        protected override bool StartHidden => true;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.5f,
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding(20),
                    Children = new Drawable[]
                    {
                        new GridContainer
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
                                            new SpriteText
                                            {
                                                Anchor = Anchor.BottomLeft,
                                                Origin = Anchor.BottomLeft,
                                                Text = @"Prioridad:",
                                                Font = new FontUsage(size: 20),
                                            },
                                            priorityBox = new NumericTextBox(false)
                                            {
                                                LengthLimit = 1,
                                                Size = new Vector2(100, 30),
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
                                            OnSelection = created => Current.Value.Actions.Add(Activator.CreateInstance(created.GetType()) as EventAction),
                                            Anchor = Anchor.BottomCentre,
                                            Origin = Anchor.BottomCentre,
                                        },
                                    },
                                },
                            },
                        },
                        new GamesToGoButton
                        {
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            Height = 35,
                            Width = 200,
                            Text = @"Cerrar",
                            Action = Hide,
                        },
                    },
                },
                argumentListing,
                selectorOverlay,
            };
        }

        public void ShowEvent(ProjectEvent model)
        {
            Current.Value?.Name.UnbindAll();

            Current.Value = model;
            eventNameBox.Text = model.Name.Value;
            Current.Value.Name.BindTo(eventNameBox.Current);
            priorityBox.Current.UnbindAll();
            priorityBox.Text = model.Priority.ToString();
            priorityBox.Current.BindValueChanged(obj => Current.Value.Priority.Value = (int)obj.NewValue, true);

            Current.Value.Actions.CollectionChanged += (_, _) => recreateActions();

            eventDescriptorContainer.Child = new EventDescriptor(model);

            recreateActions();

            Show();
        }

        private void recreateActions()
        {
            actionFillFlow.Clear();
            foreach (var action in Current.Value.Actions)
            {
                actionFillFlow.Add(new ActionDescriptor(action)
                {
                    RemoveAction = a => Current.Value.Actions.Remove(a),
                });
            }
        }

        protected override void PopIn()
        {
            this.FadeIn(250);
        }

        protected override void PopOut()
        {
            this.FadeOut(250);
        }
    }
}
