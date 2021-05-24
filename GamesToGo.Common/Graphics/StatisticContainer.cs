using GamesToGo.Common.Online.RequestModel;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;

namespace GamesToGo.Common.Graphics
{
    public class StatisticContainer : Container
    {
        private Statistic stat;
        private FillFlowContainer boxes;

        public StatisticContainer(Statistic stat)
        {
            this.stat = stat;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Child = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                Children = new Drawable[]
                {
                    new SpriteText
                    {
                        Text = stat.Name,
                        Font = new FontUsage(size: 40)
                    },
                    boxes = new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.Y,
                        Height = .7f,
                        AutoSizeAxes = Axes.X,
                        Direction = FillDirection.Horizontal
                    }
                }
            };
            populateBoxes();
        }

        private void populateBoxes()
        {
            for(int i = 0; i < stat.Amount.ToString().Length; i++)
            {
                boxes.Add(new Container
                {
                    RelativeSizeAxes = Axes.Y,
                    Width = 150,
                    BorderColour = Colour4.Black,
                    BorderThickness = .5f,
                    Masking = true,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Colour4.LightGray
                        },
                        new SpriteText
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Text = stat.Amount.ToString()[i].ToString(),
                            Font = new FontUsage(size: 250)
                        }
                    }
                });
            }
        }
    }
}
