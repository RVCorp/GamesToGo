using System;
using System.Linq;
using GamesToGo.Desktop.Project.Actions;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Graphics
{
    public class GameConditionalDescriptor : Container
    {
        private BasicScrollContainer scrollContainer;
        private FillFlowContainer descriptionContainer;
        public EventAction Model;
        private Func<EventAction, bool> Function;

        public GameConditionalDescriptor(EventAction model, Func<EventAction,bool> func)
        {
            Model = model;
            Function = func;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Red,
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    RowDimensions = new []
                    {
                        new Dimension(GridSizeMode.AutoSize)
                    },
                    ColumnDimensions = new []
                    {
                        new Dimension(),
                        new Dimension(GridSizeMode.AutoSize)
                    },
                    Content = new []
                    {
                        new Drawable[]
                        {
                            new Container
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Child = scrollContainer = new BasicScrollContainer(Direction.Horizontal)
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    ScrollbarOverlapsContent = false,
                                    Child = descriptionContainer = new FillFlowContainer
                                     {
                                         Anchor = Anchor.CentreLeft,
                                         Origin = Anchor.CentreLeft,
                                         AutoSizeAxes = Axes.Both,
                                         Direction = FillDirection.Horizontal,
                                     },
                                },
                            },
                            new Container
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Child = new IconButton(FontAwesome.Solid.TrashAlt, Colour4.DarkRed)
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Action = () => Function(Model)
                                }
                            }
                        }
                    }
                },                
            };

            scrollContainer.ScrollContent.RelativeSizeAxes = Axes.None;
            scrollContainer.ScrollContent.AutoSizeAxes = Axes.Both;

            for (int i = 0; i < Model.ExpectedArguments.Length; i++)
            {
                descriptionContainer.AddRange(new Drawable[]
                {
                    new SpriteText
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Padding = new MarginPadding(4),
                        Text = Model.Text[i],
                        Font = new FontUsage(size: 25),
                    },
                    new ArgumentChanger(Model.ExpectedArguments[i], Model.Arguments[i]),
                });
            }

            if (Model.ExpectedArguments.Length < Model.Text.Length)
            {
                descriptionContainer.Add(new SpriteText
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Padding = new MarginPadding(4),
                    Text = Model.Text.Last(),
                    Font = new FontUsage(size: 25),
                });
            }
        }
    }
}
