using System;
using System.Linq;
using GamesToGo.Desktop.Overlays;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Project.Events;
using GamesToGo.Desktop.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Graphics
{
    public class ActionTypeListing : Container
    {
        private readonly IBindable<ProjectElement> currentEditing = new Bindable<ProjectElement>();
        private ActionListContainer possibleEventsList;

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
                            Text = @"Añadir acción",
                            Action = toggleAvailableEvents,
                        },
                    },
                    possibleEventsList = new ActionListContainer(),
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

            if (element == null)
                return;

            foreach (var type in WorkingProject.AvailableActions)
            {
                var defaultAction = Activator.CreateInstance(type) as EventAction;
                possibleEventsList.AddPossibility(defaultAction);
            }
        }

        private class ActionListContainer : VisibilityContainer
        {
            private FillFlowContainer<ActionTypeList> lists;

            protected override bool StartHidden => true;

            [BackgroundDependencyLoader]
            private void load()
            {
                Alpha = 1;
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
                        lists = new FillFlowContainer<ActionTypeList>
                        {
                            AutoSizeAxes = Axes.Both,
                            Direction = FillDirection.Horizontal,
                        },
                    },
                };

                lists.AddRange(new []
                {
                    new ActionTypeList(@"Jugador", ArgumentType.Player),
                    new ActionTypeList(@"Cartas", ArgumentType.Card),
                    new ActionTypeList(@"Casillas", ArgumentType.Tile),
                    new ActionTypeList(@"Tableros", ArgumentType.Board),
                    new ActionTypeList(@"Fichas", ArgumentType.Token),
                    new ActionTypeList(@"Otros", ArgumentType.Default),
                });
            }

            protected override void PopIn()
            {
                Child.FadeIn();
            }

            protected override void PopOut()
            {
                Child.FadeOut();
            }

            public void AddPossibility(EventAction defaultEvent)
            {
                if (defaultEvent.ExpectedArguments.Length == 0)
                {
                    lists.Last().AddPossibility(defaultEvent);

                    return;
                }

                foreach (var list in lists)
                {
                    foreach(var arg in defaultEvent.ExpectedArguments)
                    {
                        if ((arg & list.ExpectedType) > 0)
                            list.AddPossibility(defaultEvent);
                    }
                }
            }
        }

        private class ActionTypeList : Container
        {
            private readonly FillFlowContainer possibilitiesList;
            public ArgumentType ExpectedType { get; }

            public ActionTypeList(string title, ArgumentType type)
            {
                ExpectedType = type;
                AutoSizeAxes = Axes.Both;
                InternalChildren = new Drawable[]
                {
                    possibilitiesList = new FillFlowContainer
                    {
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Vertical,
                        Spacing = Vector2.Zero,
                        Children = new Drawable[]
                        {
                            new SpriteText
                            {
                                Font = new FontUsage(weight: "bold", size: 30),
                                Text = title,
                            },
                        },
                    },
                };
            }

            public void AddPossibility(EventAction defaultEvent)
            {
                possibilitiesList.Add(new ActionTypeButton(defaultEvent));
            }
        }

        private class ActionTypeButton : GamesToGoButton
        {
            private readonly Container content = new Container
            {
                RelativeSizeAxes = Axes.Y,
                AutoSizeAxes = Axes.X,
            };

            private readonly EventAction type;

            [Resolved]
            private EventEditionOverlay events { get; set; }

            public ActionTypeButton(EventAction type)
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
                Action = () => events.AddActionToCurrent(Activator.CreateInstance(type.GetType()) as EventAction);
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
