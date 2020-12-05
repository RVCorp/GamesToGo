using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using GamesToGo.Game.Graphics;
using GamesToGo.Game.Online;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;

namespace GamesToGo.Game.Overlays
{
    public class RegisterOverlay : OverlayContainer
    {
        private BasicTextBox usernameBox;
        private BasicPasswordTextBox passwordBox;
        private Container content;
        private Box shadowBox;
        private GamesToGoButton register;
        private BasicPasswordTextBox confirmPasswordBox;
        private BasicTextBox emailBox;

        [Resolved]
        private APIController api { get; set; }
        [Resolved]
        private SplashInfoOverlay infoOverlay { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                shadowBox = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Black,
                    Alpha = 0,
                },
                content = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new GridContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            RowDimensions = new[]
                            {
                                new Dimension(GridSizeMode.Relative, .2f),
                                new Dimension(GridSizeMode.Relative, .5f),
                                new Dimension()
                            },
                            ColumnDimensions = new[]
                            {
                                new Dimension()
                            },
                            Content = new[]
                            {
                                new Drawable[]
                                {
                                    new Container
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Padding = new MarginPadding(10),
                                        Child = new SimpleIconButton(FontAwesome.Solid.Times)
                                        {
                                            Anchor = Anchor.TopRight,
                                            Origin = Anchor.TopRight,
                                            Action = Hide,
                                        },
                                    },
                                },
                                new Drawable[]
                                {
                                    new Container
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Children = new Drawable[]
                                        {
                                            new FillFlowContainer
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                Direction = FillDirection.Vertical,
                                                Padding = new MarginPadding(30),
                                                Children = new Drawable[]
                                                {
                                                    new FillFlowContainer
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
                                                                Text = @"Email:",
                                                                Font = new FontUsage(size:60)
                                                            },
                                                            emailBox = new BasicTextBox
                                                            {
                                                                Origin = Anchor.TopLeft,
                                                                Anchor = Anchor.TopLeft,
                                                                Height = 150,
                                                                RelativeSizeAxes = Axes.X,
                                                            },
                                                        }
                                                    },
                                                    new FillFlowContainer
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
                                                                Text = @"Nombre de Usuario:",
                                                                Font = new FontUsage(size:60)
                                                            },
                                                            usernameBox = new BasicTextBox
                                                            {
                                                                Origin = Anchor.TopLeft,
                                                                Anchor = Anchor.TopLeft,
                                                                Height = 150,
                                                                RelativeSizeAxes = Axes.X,
                                                            },
                                                        }
                                                    },
                                                    new FillFlowContainer
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
                                                                Text = @"Contraseña:",
                                                                Font = new FontUsage(size:60),
                                                            },
                                                            passwordBox = new BasicPasswordTextBox
                                                            {
                                                                Origin = Anchor.TopLeft,
                                                                Anchor = Anchor.TopLeft,
                                                                Height = 150,
                                                                RelativeSizeAxes = Axes.X,
                                                            },
                                                        },
                                                    },
                                                    new FillFlowContainer
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
                                                                Text = @"Confirmar Contraseña:",
                                                                Font = new FontUsage(size:60),
                                                            },
                                                            confirmPasswordBox = new BasicPasswordTextBox
                                                            {
                                                                Origin = Anchor.TopLeft,
                                                                Anchor = Anchor.TopLeft,
                                                                Height = 150,
                                                                RelativeSizeAxes = Axes.X,
                                                            },
                                                        },
                                                    },
                                                },
                                            },
                                        },
                                    },
                                },
                                new Drawable[]
                                {
                                    new Container
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Padding = new MarginPadding(30),
                                        Child = register = new GamesToGoButton
                                        {
                                            Anchor = Anchor.TopCentre,
                                            Origin = Anchor.TopCentre,
                                            Height = 225,
                                            RelativeSizeAxes = Axes.X,
                                            Text = @"Registrarse",
                                            Action = () => registerUser(),
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
            };
            register.SpriteText.Font = new FontUsage(size: 60);
            register.Enabled.Value = false;

            passwordBox.Current.BindValueChanged(checkUserPass);
            confirmPasswordBox.Current.BindValueChanged(checkUserPass);
            emailBox.Current.BindValueChanged(checkUserPass);
            usernameBox.Current.BindValueChanged(checkUserPass);
        }

        private void checkUserPass(ValueChangedEvent<string> obj)
        {
            if (string.IsNullOrEmpty(passwordBox.Text) || string.IsNullOrWhiteSpace(passwordBox.Text) || string.IsNullOrWhiteSpace(usernameBox.Text) || string.IsNullOrWhiteSpace(usernameBox.Text) ||
                string.IsNullOrEmpty(emailBox.Text) || string.IsNullOrWhiteSpace(emailBox.Text) || string.IsNullOrEmpty(confirmPasswordBox.Text) || string.IsNullOrWhiteSpace(confirmPasswordBox.Text)
                || !new Regex("[^ ]{1,}\\@[^ ]{1,}\\.[^ ]{2,}").IsMatch(emailBox.Text) || passwordBox.Text != confirmPasswordBox.Text)
                register.Enabled.Value = false;
            else
                register.Enabled.Value = true;
        }

        private void registerUser()
        {
            var req = new AddUserRequest(new PasswordedUser { Email = emailBox.Text, Password = passwordBox.Text, Username = usernameBox.Text });
            req.Success += _ => registerSuccess();
            req.Failure += registerFailure;
            api.Register(req);
        }

        private void registerSuccess()
        {
            infoOverlay.Show(@"El usuario fue añadido exitosamente, intenta iniciar sesión", Colour4.Green);
        }

        private void registerFailure(Exception e)
        {
            infoOverlay.Show(
                e.Message == "BadRequest"
                    ? @"El usuario o correo ya están en uso"
                    : @"Hubo un problema al registrar al usuario", Colour4.Red);
        }

        protected override void PopIn()
        {
            shadowBox.FadeTo(0.9f, 250);
            content.FadeIn(250);
        }

        protected override void PopOut()
        {
            shadowBox.FadeOut(250, Easing.OutExpo);
            content.FadeOut(250);
        }
    }
}
