using GamesToGo.Game.Online.Models.RequestModel;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace GamesToGo.Game.Graphics
{
    public class PlayerPreview : Container
    {
        public readonly Player Model;

        [Resolved]
        private TextureStore textures { get; set; }

        public PlayerPreview(Player model)
        {
            Model = model;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Y;
            Width = 180;

            Child = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                Children = new Drawable[]
                {
                    new CircularContainer
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        BorderColour = Colour4.Black,
                        BorderThickness = 3.5f,
                        Masking = true,
                        Size = new Vector2(100),
                        Child = new Sprite
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                            Texture = textures.Get($"https://gamestogo.company/api/Users/DownloadImage/{Model.BackingUser.ID}"),
                        },
                    },
                    new SpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Text = Model.BackingUser.Username,
                        Font = new FontUsage(size: 40),
                    },
                },
            };
        }
    }
}
