using System;
using System.Text.RegularExpressions;
using GamesToGo.Common.Online;
using GamesToGo.Common.Overlays;
using GamesToGo.Editor.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;

namespace GamesToGo.Editor.Overlays
{
    public class RegisterOverlay : OverlayContainer
    {
        private Box shadowBox;
        private Container popUpContent;
        private BasicTextBox usernameBox;
        private BasicTextBox emailBox;
        private BasicPasswordTextBox passwordBox;
        private BasicPasswordTextBox confirmPasswordBox;
        private GamesToGoButton registerButton;
        [Resolved]
        private APIController api { get; set; }
        [Resolved]
        private SplashInfoOverlay infoOverlay { get; set; }
        private readonly Colour4 confirmationColor = new Colour4(47, 69, 33, 255);

        [BackgroundDependencyLoader]
        private void load()
        {
            Origin = Anchor.TopLeft;
            Anchor = Anchor.TopLeft;
            RelativeSizeAxes = Axes.Both;
            Width = 1 / 3f;
            Children = new Drawable[]
            {
                shadowBox = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Black,
                    Alpha = 0,
                },
                popUpContent = new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.X,
                    X = 1,
                    Child = new Container
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        AutoSizeAxes = Axes.Y,
                        RelativeSizeAxes = Axes.X,
                        Width = 3/4f,
                        BorderColour = new Colour4(70, 68, 66, 255),
                        BorderThickness = 4,
                        Masking = true,
                        CornerRadius = 15,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = new Colour4(106, 100, 104, 255),
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
                                            Text = @"Usuario:",
                                        },
                                        usernameBox = new BasicTextBox
                                        {
                                            Origin = Anchor.TopLeft,
                                            Anchor = Anchor.TopLeft,
                                            Height = 35,
                                            Width = 380,
                                            Margin = new MarginPadding{ Bottom = 20 },
                                        },
                                        new SpriteText
                                        {
                                            Origin = Anchor.TopLeft,
                                            Anchor = Anchor.TopLeft,
                                            Text = @"Correo:",
                                        },
                                        emailBox = new BasicTextBox
                                        {
                                            Origin = Anchor.TopLeft,
                                            Anchor = Anchor.TopLeft,
                                            Height = 35,
                                            Width = 380,
                                            Margin = new MarginPadding{ Bottom = 20 },
                                        },
                                        new SpriteText
                                        {
                                            Origin = Anchor.TopLeft,
                                            Anchor = Anchor.TopLeft,
                                            Text = @"Contraseña:",
                                        },
                                        passwordBox = new BasicPasswordTextBox
                                        {
                                            Origin = Anchor.TopLeft,
                                            Anchor = Anchor.TopLeft,
                                            Height = 35,
                                            Width = 380,
                                            Margin = new MarginPadding { Bottom = 20 },
                                        },
                                        new SpriteText
                                        {
                                            Origin = Anchor.TopLeft,
                                            Anchor = Anchor.TopLeft,
                                            Text = @"Verificar Contraseña:",
                                        },
                                        confirmPasswordBox = new BasicPasswordTextBox
                                        {
                                            Origin = Anchor.TopLeft,
                                            Anchor = Anchor.TopLeft,
                                            Height = 35,
                                            Width = 380,
                                            Margin = new MarginPadding{ Bottom = 20 },
                                        },
                                        new Container
                                        {
                                            Origin = Anchor.TopLeft,
                                            Anchor = Anchor.TopLeft,
                                            RelativeSizeAxes = Axes.X,
                                            AutoSizeAxes = Axes.Y,
                                            Child = registerButton = new GamesToGoButton
                                            {
                                                Origin = Anchor.BottomCentre,
                                                Anchor = Anchor.BottomCentre,
                                                Text = @"Registrarse",
                                                Width = 100,
                                                Height = 35,
                                                Action = registerUser,
                                            },
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
            };

            registerButton.Enabled.Value = false;

            passwordBox.Current.BindValueChanged(checkUserPass);
            usernameBox.Current.BindValueChanged(checkUserPass);
            confirmPasswordBox.Current.BindValueChanged(checkUserPass);
            emailBox.Current.BindValueChanged(checkUserPass);
        }

        private void registerUser() => api.Register(usernameBox.Text, emailBox.Text, passwordBox.Text, _ => registerSuccess(), registerFailure);

        private void registerSuccess()
        {
            infoOverlay.Show(@"El usuario fue añadido exitosamente, intenta iniciar sesión", confirmationColor);
        }

        private void registerFailure(Exception e)
        {
            infoOverlay.Show(
                e.Message == "BadRequest"
                    ? @"El usuario o correo ya están en uso"
                    : @"Hubo un problema al registrar al usuario", confirmationColor);
        }

        public void Reset()
        {
            passwordBox.Text = "";
            usernameBox.Text = "";
            emailBox.Text = "";
            confirmPasswordBox.Text = "";
        }

        private void checkUserPass(ValueChangedEvent<string> obj)
        {
            if (string.IsNullOrEmpty(passwordBox.Text) || string.IsNullOrWhiteSpace(passwordBox.Text) || string.IsNullOrWhiteSpace(usernameBox.Text) || string.IsNullOrWhiteSpace(usernameBox.Text) ||
                string.IsNullOrEmpty(emailBox.Text) || string.IsNullOrWhiteSpace(emailBox.Text) || string.IsNullOrEmpty(confirmPasswordBox.Text) || string.IsNullOrWhiteSpace(confirmPasswordBox.Text)
                || !new Regex("[^ ]{1,}\\@[^ ]{1,}\\.[^ ]{2,}").IsMatch(emailBox.Text) || passwordBox.Text != confirmPasswordBox.Text)
                registerButton.Enabled.Value = false;
            else
                registerButton.Enabled.Value = true;
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
