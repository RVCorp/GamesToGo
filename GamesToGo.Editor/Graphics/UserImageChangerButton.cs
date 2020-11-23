using System.IO;
using GamesToGo.Editor.Online;
using GamesToGo.Editor.Overlays;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osuTK;

namespace GamesToGo.Editor.Graphics
{
    public class UserImageChangerButton : Button
    {
        private Container hoverContainer;
        private Sprite image;
        public Vector2 ButtonSize;

        [BackgroundDependencyLoader]
        private void load(TextureStore textures, APIController api, ImageFinderOverlay imageFinder, SplashInfoOverlay infoOverlay)
        {
            AutoSizeAxes = Axes.Both;
            Child = new CircularContainer
            {
                BorderColour = Colour4.Black,
                BorderThickness = 3.5f,
                Masking = true,
                Size = ButtonSize,
                Children = new Drawable[]
                {
                    image = new Sprite
                    {
                        RelativeSizeAxes = Axes.Both,
                        Texture = textures.Get($"https://gamestogo.company/api/Users/DownloadImage/{api.LocalUser.Value.ID}"),
                    },
                    hoverContainer = new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0,
                        Children = new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = Colour4.Black.Opacity(0.5f),
                            },
                            new SpriteIcon
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.BottomCentre,
                                Icon = FontAwesome.Regular.Images,
                                Size = new Vector2(60),
                            },
                            new SpriteText
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.TopCentre,
                                Font = new FontUsage(size: 40),
                                Text = @"Cambiar imagen",
                            },
                        },
                    },
                },
            };

            Action = () => imageFinder.Show(i =>
            {
                var req = new UploadUserImageRequest(i);
                req.Success += () => image.Texture = Texture.FromStream(new MemoryStream(i));
                req.Failure += e => infoOverlay.Show(@"Falló como siempre", Colour4.DarkRed);
                api.Queue(req);
            });
        }

        protected override bool OnHover(HoverEvent e)
        {
            hoverContainer.FadeIn(125);
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            hoverContainer.FadeOut(125);
        }
    }
}
