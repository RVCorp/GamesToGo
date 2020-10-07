using System.Globalization;
using System.Linq;
using GamesToGo.Desktop.Graphics;
using GamesToGo.Desktop.Overlays;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Project.Elements;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osuTK;

namespace GamesToGo.Desktop.Screens
{
    public class ProjectObjectScreen : Screen
    {
        private readonly IBindable<ProjectElement> currentEditing = new Bindable<ProjectElement>();

        [Resolved]
        private WorkingProject project { get; set; }
        private BasicTextBox nameTextBox;
        private Container noSelectionContainer;
        private BasicScrollContainer activeEditContainer;
        private Container elementSize;
        private NumericTextBox sizeTextBoxX;
        private NumericTextBox sizeTextBoxY;
        private Container elementSubElements;
        private BoardObjectManagerContainer tilesManagerContainer;
        private BasicTextBox descriptionTextBox;

        [Cached]
        private TileEditorOverlay tileOverlay = new TileEditorOverlay();

        [BackgroundDependencyLoader]
        private void load(ProjectEditor editor)
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Colour4 (106, 100, 104, 255),
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Content = new[]
                    {
                        new Drawable[]
                        {
                            //Listas de elementos
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable []
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = Colour4.Gray,
                                    },
                                    new ProjectObjectManagerContainer<Card>(true)
                                    {
                                        Anchor = Anchor.TopLeft,
                                        Origin = Anchor.TopLeft,
                                        Height = 1/3f,
                                    },
                                    new ProjectObjectManagerContainer<Token>(true)
                                    {
                                        Anchor = Anchor.CentreLeft,
                                        Origin = Anchor.CentreLeft,
                                        Height = 1/3f,
                                    },
                                    new ProjectObjectManagerContainer<Board>(true)
                                    {
                                        Anchor = Anchor.BottomLeft,
                                        Origin = Anchor.BottomLeft,
                                        Height = 1/3f,
                                    },
                                },
                            },
                            //Area de edición
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Children = new Drawable[]
                                {
                                    activeEditContainer = new BasicScrollContainer
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Child = new FillFlowContainer
                                        {
                                            RelativeSizeAxes = Axes.X,
                                            AutoSizeAxes = Axes.Y,
                                            Direction = FillDirection.Vertical,
                                            Children = new Drawable[]
                                            {
                                                new Container
                                                {
                                                    RelativeSizeAxes = Axes.X,
                                                    AutoSizeAxes = Axes.Y,
                                                    Children = new Drawable[]
                                                    {
                                                        new Box
                                                        {
                                                            RelativeSizeAxes = Axes.Both,
                                                            Colour = Colour4.Cyan,
                                                        },
                                                        new Container
                                                        {
                                                            RelativeSizeAxes = Axes.X,
                                                            AutoSizeAxes = Axes.Y,
                                                            Padding = new MarginPadding { Horizontal = 60, Vertical = 50 },
                                                            Children = new Drawable[]
                                                            {
                                                                new ImagePreviewContainer(),
                                                                new SpriteText
                                                                {
                                                                    Anchor = Anchor.TopRight,
                                                                    Origin = Anchor.TopRight,
                                                                    Position = new Vector2(-675,10),
                                                                    Text = @"Nombre:",
                                                                    Colour = Colour4.Black,
                                                                },
                                                                nameTextBox = new BasicTextBox
                                                                {
                                                                    Anchor = Anchor.TopRight,
                                                                    Origin = Anchor.TopRight,
                                                                    Position = new Vector2(-250,0),
                                                                    Height = 35,
                                                                    Width = 400,
                                                                },
                                                                new SpriteText
                                                                {
                                                                    Anchor = Anchor.TopRight,
                                                                    Origin = Anchor.TopRight,
                                                                    Position = new Vector2(-675,80),
                                                                    Text = @"Descripción:",
                                                                    Colour = Colour4.Black,
                                                                },
                                                                descriptionTextBox = new BasicTextBox
                                                                {
                                                                    Anchor = Anchor.TopRight,
                                                                    Origin = Anchor.TopRight,
                                                                    Position = new Vector2(-250, 70),
                                                                    Height = 35,
                                                                    Width = 400,
                                                                },
                                                            },
                                                        },
                                                    },
                                                },
                                                new Container
                                                {
                                                    RelativeSizeAxes = Axes.X,
                                                    Height = 600,
                                                    Children = new Drawable[]
                                                    {
                                                        new Box
                                                        {
                                                            RelativeSizeAxes = Axes.Both,
                                                            Colour = Colour4.Fuchsia,
                                                        },
                                                        elementSize = new Container
                                                        {
                                                            RelativeSizeAxes = Axes.Both,
                                                            Children = new Drawable[]
                                                            {
                                                                new SpriteText
                                                                {
                                                                    Text = @"Tamaño X:",
                                                                    Position = new Vector2(50, 50),
                                                                },
                                                                sizeTextBoxX = new NumericTextBox(4)
                                                                {
                                                                    Height = 35,
                                                                    Width = 75,
                                                                    Position = new Vector2(125, 45),
                                                                },
                                                                new SpriteText
                                                                {
                                                                    Text = @"Tamaño Y:",
                                                                    Position = new Vector2(50, 100),
                                                                },
                                                                sizeTextBoxY = new NumericTextBox(4)
                                                                {
                                                                    Height = 35,
                                                                    Width = 75,
                                                                    Position = new Vector2(125, 95),
                                                                },
                                                            },
                                                        },
                                                        elementSubElements = new Container
                                                        {
                                                            RelativeSizeAxes = Axes.Both,
                                                            Width = 1/3f,
                                                            Anchor = Anchor.TopRight,
                                                            Origin = Anchor.TopRight,
                                                            Child = tilesManagerContainer = new BoardObjectManagerContainer(),
                                                        },
                                                    },
                                                },
                                            },
                                        },
                                    },
                                    tileOverlay,
                                    noSelectionContainer = new Container
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        Child = new SpriteText
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Text = @"Selecciona un objeto para editarlo",
                                        },
                                    },
                                },
                            },
                        },
                    },
                    ColumnDimensions = new[]
                    {
                        new Dimension(GridSizeMode.Relative, 0.25f),
                        new Dimension(),
                    },
                },
            };
            currentEditing.BindTo(editor.CurrentEditingElement);
            currentEditing.BindValueChanged(checkData, true);

        }

        private void checkData(ValueChangedEvent<ProjectElement> obj)
        {
            activeEditContainer.FadeTo(obj.NewValue == null ? 0 : 1);
            noSelectionContainer.FadeTo(obj.NewValue == null ? 1 : 0);

            nameTextBox.Current.UnbindEvents();
            sizeTextBoxX.Current.UnbindEvents();
            sizeTextBoxY.Current.UnbindEvents();
            descriptionTextBox.Current.UnbindEvents();

            if (obj.NewValue != null)
            {
                nameTextBox.Text = obj.NewValue.Name.Value;
                descriptionTextBox.Text = obj.NewValue.Description.Value;
            }

            if(obj.NewValue is IHasSize size)
            {
                elementSize.Show();
                sizeTextBoxX.Text = size.Size.Value.X.ToString(CultureInfo.InvariantCulture);
                sizeTextBoxY.Text = size.Size.Value.Y.ToString(CultureInfo.InvariantCulture);
                sizeTextBoxX.Current.ValueChanged += sizeObj => size.Size.Value = new Vector2(float.Parse(string.IsNullOrEmpty(sizeObj.NewValue) ? sizeObj.OldValue : sizeObj.NewValue), size.Size.Value.Y);
                sizeTextBoxY.Current.ValueChanged += sizeObj => size.Size.Value = new Vector2(size.Size.Value.X, float.Parse(string.IsNullOrEmpty(sizeObj.NewValue) ? sizeObj.OldValue : sizeObj.NewValue));
            }
            else
            {
                elementSize.Hide();
            }

            if(obj.NewValue is Board board)
            {
                elementSubElements.Show();
                tilesManagerContainer.Filter = t =>
                {
                    return board.Elements.Any(ti => ti.ID == t.ID);
                };
                tilesManagerContainer.ButtonAction = () =>
                {
                    board.Elements.Add(new Tile());
                    project.AddElement(board.Elements.Last());
                };
            }
            else
            {
                elementSubElements.Hide();
            }
            tileOverlay.Hide();
            descriptionTextBox.Current.ValueChanged += text => currentEditing.Value.Description.Value = text.NewValue;
            nameTextBox.Current.ValueChanged += text => currentEditing.Value.Name.Value = text.NewValue;
        }
    }
}
