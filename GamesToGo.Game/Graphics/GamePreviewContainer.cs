using GamesToGo.Game.Online;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace GamesToGo.Game.Graphics
{
    public class GamePreviewContainer : Button
    {
        private OnlineGame game;
        private Sprite gameImage;

        public GamePreviewContainer(OnlineGame game)
        {
            this.game = game;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            RelativeSizeAxes = Axes.Both;
            Masking = true;
            BorderColour = Colour4.Black;
            BorderThickness = 3.5f;
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Colour4 (106,100,104, 255)
                },
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Direction = FillDirection.Horizontal,
                    Padding = new MarginPadding(40),
                    Spacing = new Vector2(40),
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Width = .3f,
                            Child = gameImage =  new Sprite
                            {
                                RelativeSizeAxes = Axes.Both,
                                Texture = textures.Get("Images/gtg")
                            }
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Width = .7f,
                            Padding = new MarginPadding(){ Right = 50},
                            Children = new Drawable[]
                            {
                                new FillFlowContainer
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Direction = FillDirection.Vertical,
                                    Children = new Drawable[]
                                    {
                                        new Container
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            Height = .3f,
                                            Child = new SpriteText
                                            {
                                                Text = game.Name,
                                                Font = new FontUsage(size: 100)
                                            }
                                        },
                                        new Container
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            Height = .2f,
                                            Child = new SpriteText
                                            {
                                                Text = "Hecho por: "+ game.Creator.Username,
                                                Font = new FontUsage(size: 65)
                                            },
                                        },
                                    },
                                },
                                new Container
                                {
                                    Anchor = Anchor.BottomRight,
                                    Origin = Anchor.BottomRight,
                                    RelativeSizeAxes = Axes.Both,
                                    Height = .3f,
                                    Width = .5f,
                                    Child = new FillFlowContainer
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Direction = FillDirection.Horizontal,
                                        Spacing = new Vector2(30),
                                        Children = new Drawable[]
                                        {
                                            new SpriteIcon
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                Width = .325f,
                                                Icon = FontAwesome.Solid.Users,
                                            },
                                            new SpriteText
                                            {
                                                Text = game.Minplayers + "-" + game.Maxplayers,
                                                Font = new FontUsage(size:100),
                                            },
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
            };
            Schedule(async () =>
            {
                gameImage.Texture = await textures.GetAsync(@$"https://gamestogo.company/api/Games/DownloadFile/{game.Image}");
            });
        }


    }
}
