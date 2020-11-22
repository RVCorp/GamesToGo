using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Project.Actions;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace GamesToGo.Desktop.Graphics
{
    public class VictoryConditionsContainer : VisibilityContainer
    {
        private readonly BindableList<EventAction> victoryActions = new BindableList<EventAction>();
        private FillFlowContainer<ActionDescriptor> conditionalFillFlow;
        [Resolved]
        private WorkingProject project { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.MediumPurple.Opacity(0.3f),
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    RowDimensions = new []
                    {
                        new Dimension(),
                        new Dimension(GridSizeMode.Absolute, 50),
                    },
                    ColumnDimensions = new []
                    {
                        new Dimension()
                    },
                    Content = new []
                    {
                        new Drawable[]
                        {
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Child = new BasicScrollContainer
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    ClampExtension = 30,
                                    Child = conditionalFillFlow = new FillFlowContainer<ActionDescriptor>
                                    {
                                        AutoSizeAxes = Axes.Y,
                                        RelativeSizeAxes = Axes.X,
                                        Direction = FillDirection.Vertical,
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
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = Colour4.Beige,
                                    },
                                    new GamesToGoButton
                                    {
                                        Anchor = Anchor.CentreRight,
                                        Origin = Anchor.CentreRight,
                                        Size = new Vector2(200, 35),
                                        Text = @"Añadir nuevo",
                                        Action = () => victoryActions.Add(Activator.CreateInstance(typeof(PlayerWinsAction)) as EventAction),
                                    },
                                },
                            },
                        },
                    },
                },
            };
            victoryActions.BindTo(project.VictoryConditions);
            victoryActions.BindCollectionChanged ((_, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        checkAdded(args.NewItems.Cast<EventAction>());

                        break;
                    case NotifyCollectionChangedAction.Remove:
                        checkRemoved(args.OldItems.Cast<EventAction>());

                        break;
                }
            }, true);
        }

        private void checkAdded(IEnumerable<EventAction> added)
        {
            foreach(var action in added)
            {
                conditionalFillFlow.Add(new ActionDescriptor(action)
                {
                    RemoveAction = removed => victoryActions.Remove(removed),
                });
            }
        }

        private void checkRemoved(IEnumerable<EventAction> removed)
        {
            foreach(var action in removed)
            {
                var deletable = conditionalFillFlow.FirstOrDefault(b => b.Model == action);

                if (deletable != null)
                    conditionalFillFlow.Remove(deletable);
            }
        }

        protected override void PopIn() => this.FadeIn(250);

        protected override void PopOut() => this.FadeOut(250);
    }
}
