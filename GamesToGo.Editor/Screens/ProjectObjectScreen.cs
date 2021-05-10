using System.Linq;
using GamesToGo.Editor.Graphics;
using GamesToGo.Editor.Overlays;
using GamesToGo.Editor.Project;
using GamesToGo.Editor.Project.Elements;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osuTK;

namespace GamesToGo.Editor.Screens
{
    public class ProjectObjectScreen : Screen
    {
        private readonly IBindable<ProjectElement> currentEditing = new Bindable<ProjectElement>();

        [Resolved]
        private WorkingProject project { get; set; }
        private BasicTextBox nameTextBox;
        private Container noSelectionContainer;
        private BasicScrollContainer activeEditContainer;
        private Container elementSubElements;
        private BoardObjectManagerContainer tilesManagerContainer;
        private BasicTextBox descriptionTextBox;

        [Cached]
        private TileEditorOverlay tileOverlay = new TileEditorOverlay();
        private VectorTextBoxContainer elementSize;
        private LabeledDropdown<ElementOrientation> elementOrientation;
        private LabeledDropdown<ElementPrivacy> elementPrivacy;
        private LabeledDropdown<ElementSideVisible> elementSideVisible;
        private VectorTextBoxContainer elementPosition;
        private VectorTextBoxContainer elementArrangement;
        private ElementVisualEditorContainer visualEditor;

        [BackgroundDependencyLoader]
        private void load(ProjectEditor editor)
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Colour4(106, 100, 104, 255),
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
                                Children = new Drawable[]
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
                                        Height = 1 / 3f,
                                    },
                                    new ProjectObjectManagerContainer<Token>(true)
                                    {
                                        Anchor = Anchor.CentreLeft,
                                        Origin = Anchor.CentreLeft,
                                        Height = 1 / 3f,
                                    },
                                    new ProjectObjectManagerContainer<Board>(true)
                                    {
                                        Anchor = Anchor.BottomLeft,
                                        Origin = Anchor.BottomLeft,
                                        Height = 1 / 3f,
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
                                                    Padding = new MarginPadding {Horizontal = 60, Vertical = 50},
                                                    Children = new Drawable[]
                                                    {
                                                        visualEditor = new ElementVisualEditorContainer(),
                                                        new SpriteText
                                                        {
                                                            Anchor = Anchor.TopRight,
                                                            Origin = Anchor.TopRight,
                                                            Position = new Vector2(-675, 10),
                                                            Text = @"Nombre:",
                                                        },
                                                        nameTextBox = new BasicTextBox
                                                        {
                                                            CommitOnFocusLost = true,
                                                            Anchor = Anchor.TopRight,
                                                            Origin = Anchor.TopRight,
                                                            Position = new Vector2(-250, 0),
                                                            Height = 35,
                                                            Width = 400,
                                                        },
                                                        new SpriteText
                                                        {
                                                            Anchor = Anchor.TopRight,
                                                            Origin = Anchor.TopRight,
                                                            Position = new Vector2(-675, 80),
                                                            Text = @"Descripción:",
                                                        },
                                                        descriptionTextBox = new BasicTextBox
                                                        {
                                                            CommitOnFocusLost = true,
                                                            Anchor = Anchor.TopRight,
                                                            Origin = Anchor.TopRight,
                                                            Position = new Vector2(-250, 70),
                                                            Height = 35,
                                                            Width = 400,
                                                        },
                                                        new GamesToGoButton
                                                        {
                                                            Anchor = Anchor.TopRight,
                                                            Origin = Anchor.TopRight,
                                                            Height = 35,
                                                            Width = 200,
                                                            Text = @"Borrar Elemento",
                                                            Action = () => editor.DeleteElement(currentEditing.Value),
                                                        },
                                                    },
                                                },
                                                new Container
                                                {
                                                    RelativeSizeAxes = Axes.X,
                                                    AutoSizeAxes = Axes.Y,
                                                    Children = new Drawable[]
                                                    {
                                                        new FillFlowContainer
                                                        {
                                                            RelativeSizeAxes = Axes.Both,
                                                            Direction = FillDirection.Full,
                                                            Spacing = new Vector2(10),
                                                            Padding = new MarginPadding {Horizontal = 50},
                                                            Children = new Drawable[]
                                                            {
                                                                elementSize = new VectorTextBoxContainer(4, false)
                                                                {
                                                                    TextX = @"Tamaño X:",
                                                                    TextY = @"Tamaño Y:",
                                                                },
                                                                elementOrientation = new LabeledDropdown<ElementOrientation>
                                                                {
                                                                    Text = @"Orientación:",
                                                                    Element = new GamesToGoDropdown<ElementOrientation>
                                                                    {
                                                                        Width = 200,
                                                                    },
                                                                },
                                                                elementSideVisible = new LabeledDropdown<ElementSideVisible>
                                                                {
                                                                    Text = @"Lado visible:",
                                                                    Element = new GamesToGoDropdown<ElementSideVisible>
                                                                    {
                                                                        Width = 200,
                                                                    },
                                                                },
                                                                elementPrivacy = new LabeledDropdown<ElementPrivacy>
                                                                {
                                                                    Text = @"Privacidad:",
                                                                    Element = new GamesToGoDropdown<ElementPrivacy>
                                                                    {
                                                                        Width = 200,
                                                                    },
                                                                },
                                                                elementPosition = new VectorTextBoxContainer(4, true)
                                                                {
                                                                    TextX = @"Posición en X:",
                                                                    TextY = @"Posición en Y:",
                                                                },
                                                                elementArrangement = new VectorTextBoxContainer(4, true)
                                                                {
                                                                    TextX = @"Orden en X:",
                                                                    TextY = @"Orden en Y:",
                                                                },
                                                            },
                                                        },
                                                        elementSubElements = new Container
                                                        {
                                                            RelativeSizeAxes = Axes.X,
                                                            Width = 1 / 3f,
                                                            Height = 500,
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

            nameTextBox.OnCommit += delegate
            {
                if (currentEditing.Value != null)
                    currentEditing.Value.Name.Value = nameTextBox.Text;
            };

            descriptionTextBox.OnCommit += delegate
            {
                if (currentEditing.Value != null)
                    currentEditing.Value.Description.Value = descriptionTextBox.Text;
            };

            elementPosition.Current.BindValueChanged(_ => visualEditor.UpdatePreview());
            elementOrientation.Current.BindValueChanged(_ => visualEditor.UpdatePreview());
            elementSideVisible.Current.BindValueChanged(_ => visualEditor.UpdatePreview());
            elementSize.Current.BindValueChanged(_ => visualEditor.UpdatePreview());
            tilesManagerContainer.ElementsAdded = _ => visualEditor.UpdatePreview();
            tilesManagerContainer.ElementsRemoved = _ => visualEditor.UpdatePreview();
        }

        private static void unbindBindings(ProjectElement oldElement)
        {
            if (oldElement is IHasLogicalArrangement arrangedElement)
                arrangedElement.Arrangement.UnbindAll();

            if (oldElement is IHasSize sizedElement)
                sizedElement.Size.UnbindAll();

            if (oldElement is IHasPosition positionedElement)
                positionedElement.Position.UnbindAll();

            if (oldElement is IHasOrientation orientedElement)
                orientedElement.DefaultOrientation.UnbindAll();

            if (oldElement is IHasSideVisible sidedElement)
                sidedElement.DefaultSide.UnbindAll();

            if (oldElement is IHasPrivacy privacySetElement)
                privacySetElement.DefaultPrivacy.UnbindAll();
        }

        private void checkData(ValueChangedEvent<ProjectElement> obj)
        {
            activeEditContainer.FadeTo(obj.NewValue == null ? 0 : 1);
            noSelectionContainer.FadeTo(obj.NewValue == null ? 1 : 0);

            unbindBindings(obj.OldValue);

            if (obj.NewValue != null)
            {
                nameTextBox.Text = obj.NewValue.Name.Value;
                descriptionTextBox.Text = obj.NewValue.Description.Value;
            }

            if (obj.NewValue is IHasSize size)
            {
                elementSize.Show();
                elementSize.Current.Value = size.Size.Value;
                size.Size.BindTo(elementSize.Current);
            }
            else
                elementSize.Hide();

            if (obj.NewValue is IHasOrientation orientation)
            {
                elementOrientation.Show();
                elementOrientation.Current.Value = orientation.DefaultOrientation.Value;
                orientation.DefaultOrientation.BindTo(elementOrientation.Current);
            }
            else
                elementOrientation.Hide();

            if (obj.NewValue is IHasSideVisible sideVisible)
            {
                elementSideVisible.Show();
                elementSideVisible.Current.Value = sideVisible.DefaultSide.Value;
                sideVisible.DefaultSide.BindTo(elementSideVisible.Current);
            }
            else
                elementSideVisible.Hide();

            if (obj.NewValue is IHasPrivacy privacy)
            {
                elementPrivacy.Show();
                elementPrivacy.Current.Value = privacy.DefaultPrivacy.Value;
                privacy.DefaultPrivacy.BindTo(elementPrivacy.Current);
            }
            else
                elementPrivacy.Hide();

            if (obj.NewValue is IHasPosition position)
            {
                elementPosition.Show();
                elementPosition.Current.Value = position.Position.Value;
                position.Position.BindTo(elementPosition.Current);
            }
            else
                elementPosition.Hide();

            if (obj.NewValue is IHasLogicalArrangement arrangement)
            {
                elementArrangement.Show();
                elementArrangement.Current.Value = arrangement.Arrangement.Value;
                arrangement.Arrangement.BindTo(elementArrangement.Current);
            }
            else
                elementArrangement.Hide();

            if (obj.NewValue is Board board)
            {
                elementSubElements.Show();
                tilesManagerContainer.Filter = t =>
                {
                    return board.Elements.Any(ti => ti.ID == t.ID);
                };
                tilesManagerContainer.ButtonAction = () =>
                {
                    var toBeAdded = new Tile();
                    board.Elements.Add(toBeAdded);
                    toBeAdded.Parent = board;
                    project.AddElement(board.Elements.Last());
                };
            }
            else
            {
                elementSubElements.Hide();
            }

            if (obj.NewValue != tileOverlay.SelectedTile)
            {
                tileOverlay.Hide();
            }
        }
    }
}
