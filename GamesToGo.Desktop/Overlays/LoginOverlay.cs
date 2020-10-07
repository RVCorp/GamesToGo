using System;
using GamesToGo.Desktop.Graphics;
using GamesToGo.Desktop.Online;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;

namespace GamesToGo.Desktop.Overlays
{
    public class LoginOverlay : OverlayContainer
    {
        private Box shadowBox;
        private Container popUpContent;
        private BasicTextBox usernameBox;
        private BasicPasswordTextBox passwordBox;
        private GamesToGoButton loginButton;
        [Resolved]
        private APIController api { get; set; }
        private readonly Action nextScreenAction;
        private readonly Bindable<User> localUser = new Bindable<User>();

        public LoginOverlay(Action nextScreen)
        {
            nextScreenAction = nextScreen;
        }

        public void Reset()
        {
            passwordBox.Text = string.Empty;
            usernameBox.Text = string.Empty;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Origin = Anchor.TopRight;
            Anchor = Anchor.TopRight;
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
                                            Text = @"Contraseña:",
                                        },
                                        passwordBox = new BasicPasswordTextBox
                                        {
                                            Origin = Anchor.TopLeft,
                                            Anchor = Anchor.TopLeft,
                                            Height = 35,
                                            Width = 380,
                                            Margin = new MarginPadding { Bottom = 50 },
                                        },
                                        new Container
                                        {
                                            Origin = Anchor.TopLeft,
                                            Anchor = Anchor.TopLeft,
                                            RelativeSizeAxes = Axes.X,
                                            AutoSizeAxes = Axes.Y,
                                            Child = loginButton = new GamesToGoButton
                                            {
                                                Origin = Anchor.BottomCentre,
                                                Anchor = Anchor.BottomCentre,
                                                Text = @"Iniciar Sesión",
                                                Width = 100,
                                                Height = 35,
                                                Action = () => api.Login(usernameBox.Text, passwordBox.Text),
                                            },
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
            };

            loginButton.Enabled.Value = false;

            passwordBox.Current.BindValueChanged(checkUserPass);
            usernameBox.Current.BindValueChanged(checkUserPass);

            localUser.BindTo(api.LocalUser);
            localUser.BindValueChanged(_ => nextScreenAction?.Invoke());
            api.Login(@"daro31", @"1234");
        }

        private void checkUserPass(ValueChangedEvent<string> obj)
        {
            if (string.IsNullOrEmpty(passwordBox.Text) || string.IsNullOrWhiteSpace(passwordBox.Text) || string.IsNullOrWhiteSpace(usernameBox.Text) || string.IsNullOrWhiteSpace(usernameBox.Text))
                loginButton.Enabled.Value = false;
            else
                loginButton.Enabled.Value = true;
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
            popUpContent.MoveToX(1, 250, Easing.OutExpo);
        }
    }
}
