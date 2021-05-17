using System;
using GamesToGo.Common.Online;
using GamesToGo.Common.Online.RequestModel;
using GamesToGo.Game.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;

namespace GamesToGo.Game.Overlays
{
    public class LoginOverlay : OverlayContainer
    {
        private BasicTextBox usernameBox;
        private BasicPasswordTextBox passwordBox;
        private Container content;
        private Box shadowBox;
        private GamesToGoButton login;
        [Resolved]
        private APIController api { get; set; }
        private readonly Bindable<User> localUser = new Bindable<User>();
        private readonly Action nextScreenAction;

        public LoginOverlay(Action nextScreen)
        {
            nextScreenAction = nextScreen;
        }

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
                                                                Text = @"Usuario:",
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
                                        Child = login = new GamesToGoButton
                                        {
                                            Anchor = Anchor.TopCentre,
                                            Origin = Anchor.TopCentre,
                                            Height = 225,
                                            RelativeSizeAxes = Axes.X,
                                            Text = @"Iniciar Sesión",
                                            Action = () => api.Login(usernameBox.Text, passwordBox.Text),
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
            };
            login.SpriteText.Font = new FontUsage(size: 60);
            login.Enabled.Value = false;

            passwordBox.Current.BindValueChanged(checkUserPass);
            usernameBox.Current.BindValueChanged(checkUserPass);

            localUser.BindTo(api.LocalUser);
            localUser.BindValueChanged(_ =>
            {
                usernameBox.Text = "";
                passwordBox.Text = "";
                Hide();
                if(localUser.Value != null)
                    nextScreenAction?.Invoke();
            });
            api.Login(@"daro31", @"1234");
        }

        private void checkUserPass(ValueChangedEvent<string> obj)
        {
            if (string.IsNullOrEmpty(passwordBox.Text) || string.IsNullOrWhiteSpace(passwordBox.Text) || string.IsNullOrEmpty(usernameBox.Text) || string.IsNullOrWhiteSpace(usernameBox.Text))
                login.Enabled.Value = false;
            else
                login.Enabled.Value = true;
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
