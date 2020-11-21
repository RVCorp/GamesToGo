using System.Linq;
using GamesToGo.Desktop.Overlays;
using GamesToGo.Desktop.Graphics;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Project.Elements;
using GamesToGo.Desktop.Project.Events;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK.Graphics;
using NUnit.Framework;
using System.Collections.Generic;

namespace GamesToGo.Desktop.Screens
{
    [Cached]
    public class ProjectEventsScreen : Screen
    {
        private readonly IBindable<ProjectElement> currentEditing = new Bindable<ProjectElement>();

        [Resolved]
        private ProjectEditor editor { get; set; }

        [Resolved]
        private WorkingProject project { get; set; }

        private Container noSelectionContainer;
        private Container eventGraphicsContainer;

        [Cached]
        private EventEditionOverlay eventEditOverlay = new EventEditionOverlay();
        private EventListing eventsList;

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Color4 (106, 100, 104, 255),
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Content = new[]
                    {
                        new Drawable[]
                        {
                            //Listas de elementos
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable []
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = Color4.Gray,
                                    },
                                    new ProjectObjectManagerContainer<Card>
                                    {
                                        Anchor = Anchor.TopLeft,
                                        Origin = Anchor.TopLeft,
                                        Height = 1/2f,
                                    },
                                    new ProjectObjectListingContainer<Tile>
                                    {
                                        Anchor = Anchor.BottomLeft,
                                        Origin = Anchor.BottomLeft,
                                        Height = 1/2f,
                                    },
                                },
                            },
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable[]
                                {
                                    eventGraphicsContainer = new Container
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Children = new Drawable[]
                                        {
                                            new Container
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                Padding = new MarginPadding { Horizontal = 60, Vertical = 50 },
                                                Child = eventsList = new EventListing
                                                {
                                                    RelativeSizeAxes = Axes.Both,
                                                },
                                            },
                                        },
                                    },
                                    noSelectionContainer = new Container
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        Child = new SpriteText
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Text = @"Selecciona un objeto con eventos para editarlo",
                                        },
                                    },
                                    eventEditOverlay,
                                },
                            },
                        },
                    },
                    ColumnDimensions = new[]
                    {
                        new Dimension(GridSizeMode.Relative, 0.25f),
                        new Dimension(),
                    },
                },
            };
            currentEditing.BindTo(editor.CurrentEditingElement);
            currentEditing.BindValueChanged(checkData, true);
        }

        public void CreateEvent(ProjectEvent projectEvent)
        {
            if (!(currentEditing.Value is IHasEvents evented)) return;

            projectEvent.ID = evented.Events.LastOrDefault()?.ID + 1 ?? 1;
            evented.Events.Add(projectEvent);
            eventEditOverlay.ShowEvent(projectEvent);
        }

        public void RemoveEvent(ProjectEvent projectEvent)
        {
            if (!(currentEditing.Value is IHasEvents evented)) return;

            evented.Events.Remove(projectEvent);
        }

        private void checkData(ValueChangedEvent<ProjectElement> obj)
        {
            eventEditOverlay.Hide();

            if (!(obj.NewValue is IHasEvents))
            {
                eventGraphicsContainer.FadeTo(0);
                noSelectionContainer.FadeTo(1);

                return;
            }

            eventGraphicsContainer.FadeTo(1);
            noSelectionContainer.FadeTo(0);
        }
    }
}
