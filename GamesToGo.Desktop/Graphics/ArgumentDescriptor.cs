using System.Linq;
using GamesToGo.Desktop.Project.Events;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Graphics
{
    public class ArgumentDescriptor : Container
    {
        private readonly Argument model;
        private FillFlowContainer descriptionContainer;

        public ArgumentDescriptor(Argument model)
        {
            this.model = model;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            AutoSizeAxes = Axes.Both;
            Masking = true;
            CornerRadius = 4;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.2f,
                },
                descriptionContainer = new FillFlowContainer
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Horizontal,
                },
            };

            for (int i = 0; i < model.ExpectedArguments.Length; i++)
            {
                descriptionContainer.AddRange(new Drawable[]
                {
                    new SpriteText
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Padding = new MarginPadding(4),
                        Text = model.Text[i],
                        Font = new FontUsage(size: 25),
                    },
                    new ArgumentChanger(model.ExpectedArguments[i], model.Arguments[i]),
                });
            }

            if (model.ExpectedArguments.Length < model.Text.Length)
            {
                descriptionContainer.Add(new SpriteText
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Padding = new MarginPadding(4),
                    Text = model.Text.Last(),
                    Font = new FontUsage(size: 25),
                });
            }
        }
    }
}
