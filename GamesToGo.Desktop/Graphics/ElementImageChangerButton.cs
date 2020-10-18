using GamesToGo.Desktop.Overlays;
using GamesToGo.Desktop.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GamesToGo.Desktop.Graphics
{
    public class ElementImageChangerButton : ImageChangerButton
    {
        private readonly string imageName;
        private SpriteText changeImageText;

        public ElementImageChangerButton(string imageName, Bindable<Vector2> size) : base(size)
        {
            this.imageName = imageName;
        }

        public ElementImageChangerButton(string imageName)
        {
            this.imageName = imageName;
        }

        [BackgroundDependencyLoader]
        private void load(ProjectEditor editor, ImagePickerOverlay imagePicker)
        {
            Action = () => imagePicker.QueryImage(imageName);
            Editing.BindTo(editor.CurrentEditingElement.Value.Images[imageName]);

            TextureChanged = newTexture =>
            {
                changeImageText.Text = newTexture == null ? @"Añadir Imagen" : @"Cambiar Imagen";
            };
        }

        protected override Drawable CreateMainContent(Container contentContainer)
        {
            return new GridContainer
            {
                RelativeSizeAxes = Axes.Both,
                RowDimensions = new[]
                {
                    new Dimension(),
                    new Dimension(GridSizeMode.Absolute, 50),
                },
                Content = new[]
                {
                    new Drawable[]
                    {
                        contentContainer,
                    },
                    new Drawable[]
                    {
                        new SpriteText
                        {
                            Text = imageName,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Font = new FontUsage(size: 40),
                        },
                    },
                },
            };
        }

        protected override Drawable CreateNoSelectionContent() => new SpriteText
        {
            Anchor = Anchor.Centre,
            Origin = Anchor.Centre,
            Font = new FontUsage(size: 40),
            Text = @"No se ha agregado imagen",
        };

        protected override Container CreateHoverContent()
        {
            return new Container
            {
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    new SpriteIcon
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.BottomCentre,
                        Icon = FontAwesome.Regular.Images,
                        Size = new Vector2(60),
                    },
                    changeImageText = new SpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.TopCentre,
                        Font = new FontUsage(size: 40),
                    },
                },
            };
        }

        protected override Box CreateContentShadow() => new Box
        {
            Colour = Colour4.Black.Opacity(0),
            RelativeSizeAxes = Axes.Both,
        };
    }
}
