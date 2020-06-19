using System.Collections.Generic;
using System.Linq;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Project.Elements;
using GamesToGo.Desktop.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
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
    public class ProjectObjectManagerContainer<T> : Container where T : ProjectElement, new()
    {
        private IBindableList<ProjectElement> projectElements = new BindableList<ProjectElement>();

        public Color4 BackgroundColour
        {
            get
            {
                if (typeof(T).Equals(typeof(Card)))
                    return Color4.Maroon;
                if (typeof(T).Equals(typeof(Token)))
                    return Color4.Crimson;
                if (typeof(T).Equals(typeof(Board)))
                    return Color4.DarkSeaGreen;
                return Color4.Black;
            }
        }

        private FillFlowContainer<ElementEditButton<T>> allElements;

        private readonly string areaName;
        public ProjectObjectManagerContainer(string name)
        {
            areaName = name;
        }

        [BackgroundDependencyLoader]
        private void load(ProjectEditor editor, WorkingProject project)
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
                    Child = allElements = new FillFlowContainer<ElementEditButton<T>>
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y,
                        Direction = FillDirection.Full,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Padding = new MarginPadding { Vertical = 10 },
                        Spacing = new Vector2(5),
                    },
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
                            Action = () => project.AddElement(new T())
                        }
                    }
                }
            };

            checkAdded(project.ProjectElements);

            projectElements.BindTo(project.ProjectElements);

            projectElements.ItemsAdded += checkAdded;
            projectElements.ItemsRemoved += checkRemoved;
        }

        private void checkAdded(IEnumerable<ProjectElement> added)
        {
            foreach (var item in added)
            {
                if (item is T itemT)
                {
                    allElements.Add(new ElementEditButton<T>(itemT));
                }
            }
        }

        private void checkRemoved(IEnumerable<ProjectElement> removed)
        {
            foreach (var item in removed)
            {
                if(item is T)
                {
                    var deletable = allElements.Children.FirstOrDefault(b => b.Element.ID == item.ID);

                    if(deletable != null)
                    {
                        allElements.Remove(deletable);
                    }
                }
            }
        }
    }
}
