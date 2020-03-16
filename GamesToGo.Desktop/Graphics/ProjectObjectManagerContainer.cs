using GamesToGo.Desktop.Project;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Graphics
{
    public class ProjectObjectManagerContainer<T>: Container where T : IProjectElement
    {
        public Color4 BackgroundColour { get; set; } = Color4.Black;

        private ProjectObjectFillFlowContainer allElements;

        private readonly string areaName;
        public ProjectObjectManagerContainer(string name)
        {
            areaName = name;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = BackgroundColour.Opacity(0.3f),
                },
                new BasicScrollContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    ClampExtension = 30,
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding { Vertical = 50 },
                    Child = allElements = new ProjectObjectFillFlowContainer(),
                },
                new Box
                {
                    RelativeSizeAxes = Axes.X,
                    Colour = BackgroundColour,
                    Height = 50,
                },
                new SpriteText
                {
                    Text = areaName,
                    Font = new FontUsage(size: 45),
                    Position = new Vector2(5, 2.5f),
                    Shadow = true,
                },
                new Container
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    RelativeSizeAxes = Axes.X,
                    Height = 50,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.Beige
                        },
                        new BasicButton
                        {
                            RelativeSizeAxes = Axes.Y,
                            Anchor = Anchor.BottomRight,
                            Origin = Anchor.BottomRight,
                            Width = 70,
                            BackgroundColour = Color4.Red,
                            BorderColour = Color4.Black,
                            BorderThickness = 2.5f,
                            Masking = true,
                            Child =  new SpriteIcon
                            {
                                Icon = FontAwesome.Solid.Ad,
                                Colour = Color4.Black,
                                RelativeSizeAxes = Axes.Y,
                                Width = 40,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                            },
                            Action = () => allElements.AddElement(new TestObject())
                        }
                    }
                }
            };
        }
    }

    public class TestObject : IProjectElement
    {
        public int ID { get; set; }
        public string Name { get => "test"; set { } }
        public Drawable Miniature { get => null; set { } }
    }
}
