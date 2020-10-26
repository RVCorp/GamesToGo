using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK;
using Image = GamesToGo.Desktop.Project.Image;

namespace GamesToGo.Desktop.Graphics
{
    public class ContainedImage : Container
    {
        public ContainedImage(bool showOutline, float imageCornerRadius)
        {
            this.showOutline = showOutline;
            this.imageCornerRadius = imageCornerRadius;
        }

        private readonly Bindable<Vector2> size = new Bindable<Vector2>(new Vector2(400));

        public Bindable<Vector2> ImageSize
        {
            get => size;
            set
            {
                size.UnbindBindings();
                size.BindTo(value);
            }
        }

        private readonly Bindable<Image> image = new Bindable<Image>();

        public Bindable<Image> Image
        {
            get => image;
            set
            {
                image.UnbindBindings();
                image.BindTo(value);
            }
        }

        private readonly bool showOutline;
        private readonly float imageCornerRadius;
        private Container mainContent;
        private Sprite sprite;

        public float ExpectedToRealSizeRatio => size.Value.X / OverImageContent.LayoutSize.X;

        public Container UnderImageContent { get; } = new Container
        {
            RelativeSizeAxes = Axes.Both,
            Child = new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = Colour4.Transparent,
            },
        };
        public Container OverImageContent { get; } = new Container
        {
            RelativeSizeAxes = Axes.Both,
            Name = "Over Image",
        };

        public override Anchor Origin
        {
            get
            {
                byte rot = (byte)(MathF.Abs(Rotation / 90) % 4);
                return (rot / 2 > 0 ? Anchor.y2 : Anchor.y0) | (((rot >> 1) ^ (rot & 1)) == 0 ? Anchor.x0 : Anchor.x2);
            }
            set => throw new InvalidOperationException($"Can't set Origin nor Anchor of a {nameof(ContainedImage)}");
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Masking = true;
            Add(mainContent = new Container
            {
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                RelativeSizeAxes = Axes.Both,
                FillMode = FillMode.Fit,
                Masking = true,
                Child = new Container
                {
                    Masking = true,
                    BorderColour = Colour4.White,
                    BorderThickness = showOutline ? 3.5f : 0,
                    CornerRadius = imageCornerRadius,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        UnderImageContent,
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Padding = new MarginPadding(showOutline ? 2 : 0),
                            Children = new Drawable[]
                            {
                                sprite = new Sprite
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    FillMode = FillMode.Fit,
                                },
                                OverImageContent,
                            },
                        },
                    },
                },
            });

            size.BindValueChanged(s =>
            {
                mainContent.FillAspectRatio = s.NewValue.X / s.NewValue.Y;
            }, true);

            image.BindValueChanged(i =>
            {
                sprite.Texture = i.NewValue?.Texture;
            }, true);
        }
    }
}
