using GamesToGo.Game.Graphics;
using GamesToGo.Game.Overlays;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Screens;
using osuTK;

namespace GamesToGo.Game.Screens
{
    [Cached]
    public class SessionStartScreen : Screen
    {
        private GamesToGoButton loginButton;
        private LoginOverlay loginOverlay;
        private RegisterOverlay registerOverlay;
        private GamesToGoButton registerButton;
        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            RelativeSizeAxes = Axes.Both;

            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Colour4 (106,100,104, 255)
                },
                new GridContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    RowDimensions = new []
                    {
                        new Dimension(GridSizeMode.Relative, .5f),
                        new Dimension(GridSizeMode.Relative, .25f),
                        new Dimension()
                    },
                    ColumnDimensions = new[]
                    {
                        new Dimension()
                    },
                    Content = new []
                    {
                        new Drawable[]
                        {
                            new CircularContainer
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                BorderColour = Colour4.Black,
                                BorderThickness = 3.5f,
                                Masking = true,
                                Size = new Vector2(600, 600),
                                Child = new Sprite
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Texture = textures.Get("Images/gtg")
                                }
                            }
                        },
                        new Drawable[]
                        {
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Child = loginButton = new GamesToGoButton
                                {
                                    Anchor = Anchor.BottomCentre,
                                    Origin = Anchor.BottomCentre,
                                    Height = 150,
                                    Width = 800,
                                    Text = "Iniciar Sesión",
                                    Action = () => loginOverlay.Show()
                                }
                            }
                        },
                        new Drawable[]
                        {
                            new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Child = registerButton = new GamesToGoButton
                                {
                                    Anchor = Anchor.Centre,
                                    Origin = Anchor.Centre,
                                    Height = 150,
                                    Width = 800,
                                    Text = "Registrarse",
                                    Action = () => registerOverlay.Show()
                                },
                            },
                        },
                    },
                },
                loginOverlay = new LoginOverlay(loginIntoServer),
                registerOverlay = new RegisterOverlay(),
            };
            loginButton.SpriteText.Font = new FontUsage(size:60);
            registerButton.SpriteText.Font = new FontUsage(size: 60);
        }

        private void loginIntoServer()
        {
            LoadComponentAsync(new MainMenuScreen(), this.Push);
        }
    }
}
