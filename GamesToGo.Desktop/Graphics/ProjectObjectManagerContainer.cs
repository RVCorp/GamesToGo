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
using osu.Framework.Graphics.Textures;
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
                if (typeof(T).Equals(typeof(Tile)))
                    return Color4.BlueViolet;
                return Color4.Black;
            }
        }

        private FillFlowContainer<ElementEditButton<T>> allElements;

        private readonly string areaName;
        private readonly bool shouldStartEditing;
        public ProjectObjectManagerContainer(string name, bool shouldStartEditing = false)
        {
            areaName = name;
            this.shouldStartEditing = shouldStartEditing;
        }

        [BackgroundDependencyLoader]
        private void load(ProjectEditor editor, WorkingProject project, TextureStore textures)
        {
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    RowDimensions = new Dimension[]
                    {
                        new Dimension(GridSizeMode.AutoSize),
                        new Dimension(),
                        new Dimension(GridSizeMode.AutoSize),
                    },
                    Content = new Drawable[][]
                    {
                        new Drawable[]
                        {
                            new Container
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Children = new Drawable[]
                                {
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
                                }
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
                                        Colour = BackgroundColour.Opacity(0.3f),
                                    },
                                    new BasicScrollContainer
                                    {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        ClampExtension = 30,
                                        RelativeSizeAxes = Axes.Both,
                                        Child = allElements = new FillFlowContainer<ElementEditButton<T>>
                                        {
                                            RelativeSizeAxes = Axes.X,
                                            AutoSizeAxes = Axes.Y,
                                            Direction = FillDirection.Full,
                                            Anchor = Anchor.TopCentre,
                                            Origin = Anchor.TopCentre,
                                            Padding = new MarginPadding { Vertical = 10 },
                                            Spacing = new Vector2(5),
                                        },
                                    },
                                }
                            }
                            
                        },
                        new Drawable[]
                        {
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
                                    new AddElementButton
                                    {
                                        Action = () => editor.AddElement(new T(), shouldStartEditing)
                                    }
                                }
                            }
                        }
                    }
                },
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
                if (item is T)
                {
                    var deletable = allElements.Children.FirstOrDefault(b => b.Element.ID == item.ID);

                    if (deletable != null)
                    {
                        allElements.Remove(deletable);
                    }
                }
            }
        }

        private class AddElementButton : BasicButton
        {
            private readonly Container content = new Container
            {
                RelativeSizeAxes = Axes.Y,
                AutoSizeAxes = Axes.X,
            };

            protected override Container<Drawable> Content => content;
            public AddElementButton()
            {
                AddInternal(content);
                Padding = new MarginPadding(7);
                RelativeSizeAxes = Axes.Y;
                Anchor = Anchor.BottomRight;
                Origin = Anchor.BottomRight;
                AutoSizeAxes = Axes.X;
                BackgroundColour = Color4.Black;
                HoverColour = new Color4(55, 55, 55, 255);
                Text = "Añadir nuevo";

                content.Masking = true;
                content.CornerRadius = 5;
                content.BorderThickness = 3f;
                content.BorderColour = Color4.Black;
            }

            protected override SpriteText CreateText()
            {
                var text = base.CreateText();

                text.Colour = Color4.White;
                text.Font = new FontUsage(size: 25);
                text.Margin = new MarginPadding { Horizontal = 6.5f };

                return text;
            }
        }
    }
}
