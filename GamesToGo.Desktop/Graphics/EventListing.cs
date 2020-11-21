using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using GamesToGo.Desktop.Overlays;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Project.Elements;
using GamesToGo.Desktop.Project.Events;
using GamesToGo.Desktop.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Graphics
{
    public class EventListing : Container
    {
        private readonly IBindable<ProjectElement> currentEditing = new Bindable<ProjectElement>();
        private FillFlowContainer<ProjectObjectEventContainer> eventList;

        private readonly IBindableList<ProjectEvent> localEvents = new BindableList<ProjectEvent>();

        [Resolved]
        private ProjectEditor editor { get; set; }

        [Resolved]
        private EventEditionOverlay eventEditor { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            Children = new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    BorderThickness = 3f,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.DarkGoldenrod,
                        },
                        new GridContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            RowDimensions = new[]
                            {
                                new Dimension(),
                                new Dimension(GridSizeMode.AutoSize),
                            },
                            Content = new[]
                            {
                                new Drawable[]
                                {
                                    new BasicScrollContainer
                                    {
                                        ClampExtension = 10,
                                        RelativeSizeAxes = Axes.Both,
                                        Child = eventList = new FillFlowContainer<ProjectObjectEventContainer>
                                        {
                                            Spacing = Vector2.Zero,
                                            RelativeSizeAxes = Axes.X,
                                            AutoSizeAxes = Axes.Y,
                                            Direction = FillDirection.Vertical,
                                        },
                                    },
                                },
                                new Drawable[]
                                {
                                    new EventTypeListing(),
                                },
                            },
                        },
                    },
                },
            };

            currentEditing.BindTo(editor.CurrentEditingElement);
            currentEditing.BindValueChanged(obj =>
            {
                if(obj.NewValue is IHasEvents evented)
                    recreateEvents(evented.Events);
            }, true);

        }

        private void recreateEvents(IBindableList<ProjectEvent> events)
        {
            localEvents.UnbindAll();

            eventList.Clear();
            addItems(events);

            localEvents.BindTo(events);

            localEvents.CollectionChanged += (_, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        addItems(args.NewItems.Cast<ProjectEvent>());

                        break;
                    case NotifyCollectionChangedAction.Remove:
                        RemoveItems(args.OldItems.Cast<ProjectEvent>());

                        break;
                }
            };
        }

        private void addItems(IEnumerable<ProjectEvent> events)
        {
            foreach (var e in events)
                eventList.Add(new ProjectObjectEventContainer(e));
        }

        public void RemoveItems(IEnumerable<ProjectEvent> events)
        {
            foreach (var e in events)
            {
                var deletable = eventList.First(ec => ec.Event.ID == e.ID);

                if (deletable != null)
                    eventList.Remove(deletable);
            }
        }
    }
}
