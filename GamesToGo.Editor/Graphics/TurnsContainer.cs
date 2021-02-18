using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using GamesToGo.Editor.Project.Actions;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace GamesToGo.Editor.Graphics
{
    public class TurnsContainer : Container
    {
        private FillFlowContainer<ActionDescriptor> turnsFillFlow;
        protected readonly BindableList<EventAction> TurnActions = new BindableList<EventAction>();

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
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
                        new Dimension(),
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
                                    Child = turnsFillFlow = new FillFlowContainer<ActionDescriptor>
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
                                    new ActionTypeListing
                                    {
                                        OnSelection = selected => TurnActions.Add(Activator.CreateInstance(selected.GetType()) as EventAction),
                                        Anchor = Anchor.CentreRight,
                                        Origin = Anchor.CentreRight,
                                    },
                                },
                            },
                        },
                    },
                },
            };
            TurnActions.BindCollectionChanged ((_, args) =>
            {
                switch (args.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        checkAdded(args.NewItems?.Cast<EventAction>());

                        break;
                    case NotifyCollectionChangedAction.Remove:
                        checkRemoved(args.OldItems?.Cast<EventAction>());

                        break;
                }
            }, true);
        }

        private void checkAdded(IEnumerable<EventAction> added)
        {
            foreach (var action in added)
            {
                turnsFillFlow.Add(new ActionDescriptor(action)
                {
                    RemoveAction = removed => TurnActions.Remove(removed),
                });
            }
        }

        private void checkRemoved(IEnumerable<EventAction> removed)
        {
            foreach (var action in removed)
            {
                var deletable = turnsFillFlow.FirstOrDefault(b => b.Model == action);

                if (deletable != null)
                    turnsFillFlow.Remove(deletable);
            }
        }
    }
}
