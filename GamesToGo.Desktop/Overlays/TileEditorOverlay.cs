using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Desktop.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Overlays
{
    public class TileEditorOverlay : OverlayContainer
    {
        private BasicScrollContainer activeEditContainer;
        private BasicTextBox nameTextBox;
        private Container customElementsContainer;
        private Container noSelectionContainer;

        public TileEditorOverlay()
        {
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                new GamesToGoButton
                {
                    Height = 35,
                    Width = 175,
                    Text = "Regresar",
                    Position = new Vector2(20,20)
                },
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
                                        Colour = Color4.Cyan
                                    },
                                    new Container
                                    {
                                        RelativeSizeAxes = Axes.X,
                                        AutoSizeAxes = Axes.Y,
                                        Padding = new MarginPadding() { Horizontal = 60, Vertical = 50 },
                                        Children = new Drawable[]
                                        {
                                            new ImagePreviewContainer(),
                                            new SpriteText
                                            {
                                                Anchor = Anchor.TopRight,
                                                Origin = Anchor.TopRight,
                                                Position = new Vector2(-675,10),
                                                Text = "Nombre:",
                                                Colour = Color4.Black
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
                                                Text = "Descripcion:",
                                                Colour = Color4.Black
                                            },
                                            new BasicTextBox
                                            {
                                                Anchor = Anchor.TopRight,
                                                Origin = Anchor.TopRight,
                                                Position = new Vector2(-250, 70),
                                                Height = 35,
                                                Width = 400,
                                            }
                                        }
                                    }
                                }
                            },
                            customElementsContainer = new Container
                            {
                                RelativeSizeAxes = Axes.X,
                                Height = 600,
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = Color4.Fuchsia
                                    },
                                    ElementSizex2 = new Container
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Children = new Drawable[]
                                        {
                                            new SpriteText
                                            {
                                                Text = "Tamaño X:",
                                                Position = new Vector2(50, 50)
                                            },
                                            SizeTextboxX = new NumericTextbox(4)
                                            {
                                                Height = 35,
                                                Width = 75,
                                                Position = new Vector2(125, 45)
                                            },
                                            new SpriteText
                                            {
                                                Text = "Tamaño Y:",
                                                Position = new Vector2(50, 100)
                                            },
                                            SizeTextboxY = new NumericTextbox(4)
                                            {
                                                Height = 35,
                                                Width = 75,
                                                Position = new Vector2(125, 95)
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
            };
        }

        public Container ElementSizex2 { get; }
        public NumericTextbox SizeTextboxX { get; }
        public NumericTextbox SizeTextboxY { get; }

        protected override void PopIn()
        {
            activeEditContainer.Delay(150)
                .MoveToX(0, 250, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            activeEditContainer.MoveToX(1, 250, Easing.OutExpo);
        }
    }
}
