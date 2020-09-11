using GamesToGo.Desktop.Overlays;
using GamesToGo.Desktop.Graphics;
using System.Linq;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Project.Elements;
using Microsoft.EntityFrameworkCore.Internal;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Graphics
{
    class ProjectObjectEventContainer: Container
    {
        public ProjectObjectEventContainer()
        {
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.X;
            Height = 80;
            Masking = true;
            BorderThickness = 3f;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.SlateGray
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Content = new Drawable[][]
                    {
                        new Drawable[]
                        {
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable[]
                                {
                                    new SpriteText
                                    {
                                        Margin = new MarginPadding{ Left = 5, Right = 2, Top = 2, Bottom =2},
                                        Text = "Nombre del evento JAJAJAJA salu2",
                                        Font = new FontUsage(size: 35)
                                    },
                                    new SpriteText
                                    {
                                        Anchor = Anchor.BottomLeft,
                                        Origin = Anchor.BottomLeft,
                                        Margin = new MarginPadding{ Left = 5, Right = 2, Top = 2, Bottom = 7},
                                        Text = "Aquí va la información del evento, al chile si vas a decir puras mamadas mejor duermete un rato jaja salu2",
                                        Font = new FontUsage(size: 20)
                                    },
                                }
                            },
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Child = new IconButton
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Icon = FontAwesome.Solid.Edit
                                }
                            },
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Child = new IconButton
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Icon = FontAwesome.Solid.TrashAlt
                                }
                            }
                        }
                    },
                    ColumnDimensions = new Dimension[]
                    {
                        new Dimension(GridSizeMode.Relative, 0.85f),
                        new Dimension(GridSizeMode.Relative, 0.075f),
                        new Dimension(GridSizeMode.Distributed)
                    }
                }
            };
        }
    }
}
