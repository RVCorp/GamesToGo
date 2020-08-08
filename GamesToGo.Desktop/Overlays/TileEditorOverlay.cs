using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Desktop.Graphics;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Project.Elements;
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
        public Container ElementSizex2;
        public NumericTextbox SizeTextboxX;
        public NumericTextbox SizeTextboxY;

        public TileEditorOverlay()
        {
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
                new GamesToGoButton
                {
                    Height = 35,
                    Width = 175,
                    Text = "Regresar",
                    Position = new Vector2(10,10),
                    Action = this.Hide
                },
            };
        }

        public void ShowElement(ProjectElement element)
        {
            Show();
            nameTextBox.Current.UnbindEvents();
            SizeTextboxX.Current.UnbindEvents();
            SizeTextboxY.Current.UnbindEvents();

            if (element != null)
            {
                nameTextBox.Text = element.Name.Value;
            }

            if (element is IHasSize size)
            {
                ElementSizex2.Show();
                SizeTextboxX.Text = size.Size.Value.X.ToString();
                SizeTextboxY.Text = size.Size.Value.Y.ToString();
                SizeTextboxX.Current.ValueChanged += (obj) => size.Size.Value = new Vector2(float.Parse((string.IsNullOrEmpty(obj.NewValue) ? obj.OldValue : obj.NewValue)), size.Size.Value.Y);
                SizeTextboxY.Current.ValueChanged += (obj) => size.Size.Value = new Vector2(size.Size.Value.X, float.Parse((string.IsNullOrEmpty(obj.NewValue) ? obj.OldValue : obj.NewValue)));
            }
            nameTextBox.Current.ValueChanged += (obj) => element.Name.Value = obj.NewValue;
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
