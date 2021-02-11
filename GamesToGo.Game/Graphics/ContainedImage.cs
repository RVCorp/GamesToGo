using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace GamesToGo.Game.Graphics
{
    public class ContainedImage : Container
    {

        public ContainedImage(bool showOutline, float imageCornerRadius)
        {
            this.showOutline = showOutline;
            this.imageCornerRadius = imageCornerRadius;
        }

        public Vector2 ImageSize
        {
            get => imageSize;
            set
            {
                imageSize = value;
                if(mainContent != null)
                {
                    mainContent.FillAspectRatio = imageSize.X / imageSize.Y;
                }
            }
        }
        private Vector2 imageSize = new Vector2(400);

        private readonly bool showOutline;
        private readonly float imageCornerRadius;
        private Container mainContent;
        private Sprite sprite;

        public Texture Texture
        {
            get => texture;
            set
            {
                texture = value;
                if(sprite != null)
                {
                    sprite.Texture = texture;
                }
            }
        }
        private Texture texture;

        public float ExpectedToRealSizeRatio => imageSize.X / OverImageContent.LayoutSize.X;

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
                FillAspectRatio = imageSize.X / imageSize.Y,
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
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Child = new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = Colour4.Transparent,
                            },
                        },
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
                                    Texture = Texture
                                },
                                OverImageContent,
                            },
                        },
                    },
                },
            });
        }
    }
}
