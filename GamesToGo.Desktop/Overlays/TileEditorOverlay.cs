using System.Globalization;
using GamesToGo.Desktop.Graphics;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Project.Elements;
using GamesToGo.Desktop.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace GamesToGo.Desktop.Overlays
{
    public class TileEditorOverlay : OverlayContainer
    {
        private BasicTextBox nameTextBox;
        private Container elementSize;
        private NumericTextBox sizeTextBoxX;
        private NumericTextBox sizeTextBoxY;
        private BasicTextBox descriptionTextBox;
        private ProjectEditor editor;
        private ProjectElement oldCurrentEditing;
        public ProjectElement ProjectElement;

        [BackgroundDependencyLoader]
        private void load(ProjectEditor editor)
        {
            this.editor = editor;
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                new BasicScrollContainer
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
                                                Text = @"Descripcion:",
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
                                },
                            },
                        },
                    },
                },
                new GamesToGoButton
                {
                    Height = 35,
                    Width = 175,
                    Text = @"Regresar",
                    Position = new Vector2(10,10),
                    Action = () => editor.SelectElement(oldCurrentEditing),
                },
            };
        }

        public void ShowElement(ProjectElement element)
        {
            oldCurrentEditing = editor.CurrentEditingElement.Value;
            ProjectElement = element;
            Show();
            nameTextBox.Current.UnbindEvents();
            sizeTextBoxX.Current.UnbindEvents();
            sizeTextBoxY.Current.UnbindEvents();
            descriptionTextBox.Current.UnbindEvents();

            if (element != null)
            {
                nameTextBox.Text = element.Name.Value;
                descriptionTextBox.Text = element.Description.Value;
                descriptionTextBox.Current.ValueChanged += obj => element.Description.Value = obj.NewValue;
                nameTextBox.Current.ValueChanged += obj => element.Name.Value = obj.NewValue;
            }

            if (!(element is IHasSize size))
                return;

            elementSize.Show();
            sizeTextBoxX.Text = size.Size.Value.X.ToString(CultureInfo.InvariantCulture);
            sizeTextBoxY.Text = size.Size.Value.Y.ToString(CultureInfo.InvariantCulture);
            sizeTextBoxX.Current.ValueChanged += obj => size.Size.Value = new Vector2(float.Parse(string.IsNullOrEmpty(obj.NewValue) ? obj.OldValue : obj.NewValue), size.Size.Value.Y);
            sizeTextBoxY.Current.ValueChanged += obj => size.Size.Value = new Vector2(size.Size.Value.X, float.Parse(string.IsNullOrEmpty(obj.NewValue) ? obj.OldValue : obj.NewValue));
        }

        protected override void PopIn()
        {
            this.FadeIn();
        }

        protected override void PopOut()
        {
            this.FadeOut();
        }
    }
}
