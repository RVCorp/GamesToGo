using GamesToGo.Desktop.Graphics;
using GamesToGo.Desktop.Project.Events;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Overlays
{
    public class EventEditionOverlay : OverlayContainer
    {
        private FillFlowContainer<ActionDescriptor> actionFillFlow;
        private Box shadowBox;
        private Container contentContainer;
        private BasicTextBox eventNameBox;
        private Container eventDescriptorContainer;
        private ProjectEvent currentModel;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;

            Child = new Container
            {
                RelativeSizeAxes = Axes.Both,
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
                                        RelativeSizeAxes = Axes.X,
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
                                        Child = new GamesToGoButton
                                        {
                                            Anchor = Anchor.BottomRight,
                                            Origin = Anchor.BottomRight,
                                            Text = @"Añadir accion",
                                            Height = 30,
                                            Width = 100,
                                            Action = addAction,
                                        },
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
            currentModel?.Name.UnbindAll();

            currentModel = model;
            eventNameBox.Text = model.Name.Value;
            currentModel.Name.BindTo(eventNameBox.Current);

            eventDescriptorContainer.Child = new EventDescriptor(model);

            Show();
        }

        private void addAction()
        {
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
