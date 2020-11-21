using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
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
    public class TurnsContainer : Container
    {
        private FillFlowContainer<GameConditionalDescriptor> turnsFillFlow;
        private BindableList<EventAction> turnActions = new BindableList<EventAction>();

        [Resolved]
        private WorkingProject project { get; set; }

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
                        new Dimension(GridSizeMode.Relative,.9375f),
                        new Dimension()
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
                                    Child = turnsFillFlow = new FillFlowContainer<GameConditionalDescriptor>
                                    {
                                        AutoSizeAxes = Axes.Y,
                                        RelativeSizeAxes = Axes.X,
                                        Direction = FillDirection.Vertical,
                                    },
                                },
                            }
                        },
                        new Drawable[]
                        {
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable[]
                                {
                                    new ActionTypeListing(addAction)
                                    {
                                        Anchor = Anchor.CentreRight,
                                        Origin = Anchor.CentreRight,
                                    }
                                }
                            }
                        }
                    }
                }
            };
            turnActions.BindTo(project.Turns);
            turnActions.BindCollectionChanged ((_, args) =>
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

        private bool addAction(EventAction type)
        {
            turnActions.Add(Activator.CreateInstance(type.GetType()) as EventAction);
            return true;
        }

        private void checkAdded(IEnumerable<EventAction> added)
        {
            foreach (var action in added)
            {
                turnsFillFlow.Add(new GameConditionalDescriptor(action, removeAction));
            }
        }

        private bool removeAction(EventAction toRemove)
        {
            turnActions.Remove(toRemove);
            return true;
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
