using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using GamesToGo.Desktop.Overlays;
using GamesToGo.Desktop.Project.Actions;
using ManagedBass.Fx;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GamesToGo.Desktop.Graphics
{
    public class VictoryDefeatTurnsContainer : Container
    {
        [Resolved]
        private TurnsOverlay turnsOverlay { get; set; }
        private FillFlowContainer<GameConditionalDescriptor> conditionalFillFlow;
        private BindableList<EventAction> list = new BindableList<EventAction>();


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
                                    Child = conditionalFillFlow = new FillFlowContainer<GameConditionalDescriptor>
                                    {
                                        RelativeSizeAxes = Axes.Both,
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
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = Colour4.White
                                    },
                                    new GamesToGoButton
                                    {
                                        Anchor = Anchor.CentreRight,
                                        Origin = Anchor.CentreRight,
                                        Size = new Vector2(200, 35),
                                        Text = "Añadir nuevo",
                                        Action = () => addAction()
                                    }
                                }
                            }
                        }
                    }
                }
            };
            list.CollectionChanged += (_, args) =>
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
            };
        }



        private void addAction()
        {
            list.Add(Activator.CreateInstance(typeof(PlayerWinsAction)) as EventAction);
        }

        private void checkAdded(IEnumerable<EventAction> added)
        {
            foreach(var action in added)
            {
                conditionalFillFlow.Add(new GameConditionalDescriptor(action));
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
    }
}
