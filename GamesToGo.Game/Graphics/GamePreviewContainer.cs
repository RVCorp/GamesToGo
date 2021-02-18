using GamesToGo.Common.Online.RequestModel;
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
        private readonly OnlineGame game;
        private Sprite gameImage;
        private TextFlowContainer text;

        public int GameNameSize { get; set; }
        public int MadeBySize { get; set; }

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
                    Spacing = new Vector2(20),
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
                            Width = .5f,
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
                                            Child = text = new TextFlowContainer
                                            {
                                                RelativeSizeAxes = Axes.Both
                                            }
                                        },
                                    },
                                },
                            },
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Width = .2f,
                            Padding = new MarginPadding { Right = 80 },
                            Child = new FillFlowContainer
                            {
                                RelativeSizeAxes = Axes.Both,
                                Direction = FillDirection.Horizontal,
                                Spacing = new Vector2(30),
                                Children = new Drawable[]
                                {
                                    new SpriteIcon
                                    {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        RelativeSizeAxes = Axes.Both,
                                        Width = .5f,
                                        Icon = FontAwesome.Solid.Users,
                                    },
                                    new SpriteText
                                    {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        Text = game.Minplayers + "-" + game.Maxplayers,
                                        Font = new FontUsage(size:GameNameSize),
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
            text.AddText(game.Name, f => f.Font = new FontUsage(size: GameNameSize));
            text.AddParagraph(@$"Hecho por: {game.Creator.Username}", t => t.Font = new FontUsage(size:MadeBySize));
        }


    }
}
