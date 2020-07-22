using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Overlays
{
    public class RegisterOverlay : OverlayContainer
    {
        private Box shadowBox;
        private Container popUpContent;

        public RegisterOverlay(Action registerAction)
        {
            Origin = Anchor.TopLeft;
            Anchor = Anchor.TopLeft;
            RelativeSizeAxes = Axes.Both;
            Width = 1 / 3f;
            Alpha = 0;
            Children = new Drawable[]
            {
                shadowBox = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0
                },
                popUpContent = new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.X,
                    X=1,
                    Child = new Container
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        AutoSizeAxes = Axes.Y,
                        RelativeSizeAxes = Axes.X,
                        Width = 3/4f,
                        BorderColour = new Color4(70, 68, 66, 255),
                        BorderThickness = 4,
                        Masking = true,
                        CornerRadius = 15,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = new Color4(106, 100, 104, 255)
                            },
                            new Container
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Padding = new MarginPadding(50),
                                Child = new FillFlowContainer
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Direction = FillDirection.Vertical,
                                    Children = new Drawable[]
                                    {
                                        new SpriteText
                                        {
                                            Origin = Anchor.TopLeft,
                                            Anchor = Anchor.TopLeft,
                                            Text = "Usuario:"
                                        },
                                        new BasicTextBox
                                        {
                                            Origin = Anchor.TopLeft,
                                            Anchor = Anchor.TopLeft,
                                            Height = 35,
                                            Width = 380,
                                            Margin = new MarginPadding{ Bottom = 20 }
                                        },
                                        new SpriteText
                                        {
                                            Origin = Anchor.TopLeft,
                                            Anchor = Anchor.TopLeft,
                                            Text = "Correo:"
                                        },
                                        new BasicTextBox
                                        {
                                            Origin = Anchor.TopLeft,
                                            Anchor = Anchor.TopLeft,
                                            Height = 35,
                                            Width = 380,
                                            Margin = new MarginPadding{ Bottom = 20 }
                                        },
                                        new SpriteText
                                        {
                                            Origin = Anchor.TopLeft,
                                            Anchor = Anchor.TopLeft,
                                            Text = "Contraseña:",
                                        },
                                        new BasicPasswordTextBox
                                        {
                                            Origin = Anchor.TopLeft,
                                            Anchor = Anchor.TopLeft,
                                            Height = 35,
                                            Width = 380,
                                            Margin = new MarginPadding { Bottom = 20 }
                                        },
                                        new SpriteText
                                        {
                                            Origin = Anchor.TopLeft,
                                            Anchor = Anchor.TopLeft,
                                            Text = "Verificar Contraseña:"
                                        },
                                        new BasicPasswordTextBox
                                        {
                                            Origin = Anchor.TopLeft,
                                            Anchor = Anchor.TopLeft,
                                            Height = 35,
                                            Width = 380,
                                            Margin = new MarginPadding{ Bottom = 20 }
                                        },
                                        new Container
                                        {
                                            Origin = Anchor.TopLeft,
                                            Anchor = Anchor.TopLeft,
                                            RelativeSizeAxes = Axes.X,
                                            AutoSizeAxes = Axes.Y,
                                            Child = new BasicButton
                                            {
                                                Origin = Anchor.BottomCentre,
                                                Anchor = Anchor.BottomCentre,
                                                Text = "Registrarse",
                                                Width = 100,
                                                Height = 35,
                                                Action = registerAction
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        protected override void PopIn()
        {
            shadowBox.FadeTo(0.5f, 250);
            popUpContent.Delay(150)
                .MoveToX(0, 250, Easing.OutQuint);
        }

        protected override void PopOut()
        {
            shadowBox.FadeOut(250, Easing.OutExpo);
            popUpContent.MoveToX(-1, 250, Easing.OutExpo);
        }
    }
}
