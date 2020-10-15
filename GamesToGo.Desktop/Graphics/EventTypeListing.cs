using System;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Project.Elements;
using GamesToGo.Desktop.Project.Events;
using GamesToGo.Desktop.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Graphics
{
    public class EventTypeListing : Container
    {
        private readonly IBindable<ProjectElement> currentEditing = new Bindable<ProjectElement>();
        private EventListContainer possibleEventsList;

        [Resolved]
        private ProjectEditor editor { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Child = new Container
            {
                RelativeSizeAxes = Axes.X,
                Height = 50,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.Honeydew,
                    },
                    new Container
                    {
                        Anchor = Anchor.BottomRight,
                        Origin = Anchor.BottomRight,
                        RelativeSizeAxes = Axes.Y,
                        Width = 100,
                        Child = new AutoSizeButton
                        {
                            RelativeSizeAxes = Axes.Both,
                            Text = @"Añadir evento",
                            Action = toggleAvailableEvents,
                        },
                    },
                    possibleEventsList = new EventListContainer(),
                },
            };

            currentEditing.BindTo(editor.CurrentEditingElement);
            currentEditing.BindValueChanged(obj => showAvailableEvents(obj.NewValue), true);
        }

        private void toggleAvailableEvents()
        {
            possibleEventsList.ToggleVisibility();
        }

        private void showAvailableEvents(ProjectElement element)
        {
            possibleEventsList.Hide();
            possibleEventsList.ClearPossibilities();

            if (element == null)
                return;

            EventSourceActivator targetSource = element.Type switch
            {
                ElementType.Token => EventSourceActivator.Token,
                ElementType.Card => EventSourceActivator.Card,
                ElementType.Tile => EventSourceActivator.Tile,
                ElementType.Board => EventSourceActivator.Board,
                _ => EventSourceActivator.Player,
            };

            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var type in WorkingProject.AvailableEvents.Values)
            {
                var defaultEvent = Activator.CreateInstance(type) as ProjectEvent;

                if (defaultEvent?.Source.HasFlag(targetSource) ?? false)
                    possibleEventsList.AddPossibility(defaultEvent);
            }
        }

        [Cached]
        private class EventListContainer : VisibilityContainer
        {
            private FillFlowContainer<EventTypeButton> list;

            protected override bool StartHidden => true;

            [BackgroundDependencyLoader]
            private void load()
            {
                Anchor = Anchor.TopRight;
                Origin = Anchor.BottomRight;
                AutoSizeAxes = Axes.Both;
                Child = new Container
                {
                    Masking = true,
                    AutoSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Colour4.Black,
                        },
                        list = new FillFlowContainer<EventTypeButton>
                        {
                            AutoSizeAxes = Axes.Both,
                            Direction = FillDirection.Vertical,
                        },
                    },
                };
            }

            protected override void PopIn()
            {
                Child.FadeIn();
            }

            protected override void PopOut()
            {
                Child.FadeOut();
            }

            public void AddPossibility(ProjectEvent defaultEvent)
            {
                list.Add(new EventTypeButton(defaultEvent)
                {
                    Height = 30,
                });
            }

            public void ClearPossibilities()
            {
                list.Clear();
            }
        }

        private class EventTypeButton : GamesToGoButton
        {
            private readonly Container content = new Container
            {
                RelativeSizeAxes = Axes.Y,
                AutoSizeAxes = Axes.X,
            };

            private readonly ProjectEvent type;

            [Resolved]
            private ProjectEventsScreen events { get; set; }

            [Resolved]
            private EventListContainer eventList { get; set; }

            public EventTypeButton(ProjectEvent type)
            {
                this.type = type;
            }

            protected override Container<Drawable> Content => content;

            [BackgroundDependencyLoader]
            private void load()
            {
                AddInternal(content);
                AutoSizeAxes = Axes.X;
                BackgroundColour = Colour4.Transparent;
                HoverColour = new Colour4(55, 55, 55, 255);
                Action = () =>
                {
                    eventList.Hide();
                    events.CreateEvent(Activator.CreateInstance(type.GetType()) as ProjectEvent);
                };
                SpriteText.Text = string.Join(' ', type.Text);
            }

            protected override SpriteText CreateText()
            {
                var text = base.CreateText();

                text.Colour = Colour4.White;
                text.Font = new FontUsage(size: 28);
                text.Margin = new MarginPadding { Horizontal = 6.5f };

                return text;
            }
        }
    }
}
