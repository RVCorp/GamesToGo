using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using osuTK;
using GamesToGo.Desktop.Overlays;
using osu.Framework.Input.Events;
using GamesToGo.Desktop.Graphics;

namespace GamesToGo.Desktop.Screens
{
    /// <summary>
    /// Pantalla de inicio de sesión y registro de usuarios, carga un usuario y los proyectos relacionados a él. (WIP)
    /// </summary>
    public class SessionStartScreen : Screen
    {
        private readonly LoginOverlay loginOverlay;
        private readonly RegisterOverlay registerOverlay;

        public SessionStartScreen()
        {
            RelativePositionAxes = Axes.X;
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Colour4 (106,100,104, 255),
                },
                new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Height = 300,
                    Width = 530,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                            Colour = new Colour4(80, 75, 74, 255),
                        },
                        new SpriteText
                        {
                            Text = "Bienvenido a Games To Go",
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Position = new Vector2(0,100),
                        },
                        new GamesToGoButton
                        {
                            Text = @"Registrarse",
                            BackgroundColour = new Colour4 (106,100,104, 255),  //Color Boton userInformation
                            BorderColour = Colour4.Black,
                            BorderThickness = 2f,
                            Masking = true,
                            Height = 40,
                            Width = 100,
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.TopLeft,
                            Position = new Vector2(100,200),
                            Action = showRegistration,
                        },
                        new GamesToGoButton
                        {
                            Text = @"Iniciar Sesión",
                            BackgroundColour = new Colour4 (106,100,104, 255),  //Color Boton userInformation
                            BorderColour = Colour4.Black,
                            BorderThickness = 2f,
                            Masking = true,
                            Height = 40,
                            Width = 100,
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            Position = new Vector2(-100,200),
                            Action = showLogin,
                        },
                    },
                },
                loginOverlay = new LoginOverlay(loginIntoServer),
                registerOverlay = new RegisterOverlay(),
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    RelativePositionAxes = Axes.X,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreRight,
                    Colour = new Colour4 (106,100,104, 255),
                    Width = 1,
                },
            };
        }

        public override void OnSuspending(IScreen next)
        {
            base.OnSuspending(next);

            this.MoveToX(1, 1000, Easing.InOutQuart);
        }

        public override void OnResuming(IScreen last)
        {
            base.OnResuming(last);

            this.MoveToX(0, 1000, Easing.InOutQuart);
            loginOverlay.Reset();
            registerOverlay.Reset();
            loginOverlay.Hide();
            registerOverlay.Hide();
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            loginOverlay.Hide();
            registerOverlay.Hide();
            return base.OnMouseDown(e);
        }

        private void showLogin()
        {
            loginOverlay.ToggleVisibility();
            registerOverlay.Hide();
        }

        private void showRegistration()
        {
            registerOverlay.ToggleVisibility();
            loginOverlay.Hide();
        }

        private void loginIntoServer()
        {
            LoadComponentAsync(new MainMenuScreen(), this.Push);
        }
    }
}
