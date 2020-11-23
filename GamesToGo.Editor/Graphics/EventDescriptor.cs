using System.Linq;
using GamesToGo.Editor.Project.Events;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;

namespace GamesToGo.Editor.Graphics
{
    public class EventDescriptor : Container
    {
        private FillFlowContainer descriptionContainer;
        private readonly ProjectEvent model;
        private BasicScrollContainer scrollContainer;

        public EventDescriptor(ProjectEvent model)
        {
            this.model = model;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            Masking = true;
            CornerRadius = 4;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Coral,
                },
                scrollContainer = new BasicScrollContainer(Direction.Horizontal)
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    ScrollbarVisible = false,
                    Child = descriptionContainer = new FillFlowContainer
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Horizontal,
                    },
                },
            };

            scrollContainer.ScrollContent.RelativeSizeAxes = Axes.None;
            scrollContainer.ScrollContent.AutoSizeAxes = Axes.Both;

            for (int i = 0; i < model.ExpectedArguments.Length; i++)
            {
                descriptionContainer.AddRange(new Drawable[]
                {
                    new SpriteText
                    {
                        Padding = new MarginPadding(4),
                        Text = model.Text[i],
                        Font = new FontUsage(size: 25),
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                    },
                    new ArgumentChanger(model.ExpectedArguments[i], model.Arguments[i]),
                });
            }

            if (model.ExpectedArguments.Length < model.Text.Length)
            {
                descriptionContainer.Add(new SpriteText
                {
                    Padding = new MarginPadding(4),
                    Text = model.Text.Last(),
                    Font = new FontUsage(size: 25),
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                });
            }
        }
    }
}
