using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osuTK.Graphics;
using osuTK;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Overlays;
using System;

namespace GamesToGo.Desktop.Screens
{
    /// <summary>
    /// Pantalla de inicio de sesión y registro de usuarios, carga un usuario y los proyectos relacionados a él. (WIP)
    /// </summary>
    public class SessionStartScreen : Screen
    {
        private LoginOverlay loginOverlay;
        private RegisterOverlay registerOverlay;

        public SessionStartScreen()
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Color4 (106,100,104, 255)      //Color fondo general
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
                            Colour = new Color4(80, 75, 74, 255)
                        },
                        new SpriteText
                        {
                            Text = "Bienvenido a Games To Go",
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Position = new Vector2(0,100)
                        },
                        new BasicButton
                        {
                            Text = "Registrarse",
                            BackgroundColour = new Color4 (106,100,104, 255),  //Color Boton userInformation
                            BorderColour = Color4.Black,
                            BorderThickness = 2f,
                            Masking = true,
                            Height = 40,
                            Width = 100,
                            Anchor = Anchor.TopLeft,
                            Origin = Anchor.TopLeft,
                            Position = new Vector2(100,200),
                            Action = showRegistration
                        },
                        new BasicButton
                        {
                            Text = "Iniciar Sesión",
                            BackgroundColour = new Color4 (106,100,104, 255),  //Color Boton userInformation
                            BorderColour = Color4.Black,
                            BorderThickness = 2f,
                            Masking = true,
                            Height = 40,
                            Width = 100,
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            Position = new Vector2(-100,200),
                            Action = showLogin
                        },
                    }
                },
                loginOverlay = new LoginOverlay(() => loginIntoServer()),
                registerOverlay = new RegisterOverlay(() => registerInServer())
            };
        }

        private void showLogin()
        {
            loginOverlay.ToggleVisibility();
        }

        private void showRegistration()
        {
            registerOverlay.ToggleVisibility();
        }

        private void registerInServer()
        {

        }

        private void loginIntoServer()
        {

            this.Push(new MainMenuScreen());
        }
    }
}
