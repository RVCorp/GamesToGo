using GamesToGo.Game.Online;
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
    public class GameScreen : Screen
    {
        private OnlineRoom room;
        private FillFlowContainer playersImages;
        private TextureStore textures;

        public GameScreen(OnlineRoom room)
        {
            this.room = room;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            this.textures = textures;
            RelativeSizeAxes = Axes.Both;
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Black
                },
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        new BasicScrollContainer(Direction.Horizontal)
                        {
                            RelativeSizeAxes = Axes.Both,
                            Height = .1f,
                            ScrollbarOverlapsContent = false,
                            Child = playersImages = new FillFlowContainer
                            {
                                RelativeSizeAxes = Axes.Y,
                                AutoSizeAxes = Axes.X,
                                Direction = FillDirection.Horizontal                                
                            }
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Height = .6f,
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = Colour4.Purple,
                                },
                                new SpriteText
                                {
                                    Text = "Aquí va el Tablero xdxdxd",
                                    Font = new FontUsage(size: 80)
                                }
                            }
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Height = .3f,
                            Children = new Drawable[]
                            {
                                new Box
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Colour = Colour4.Bisque
                                },
                                new FillFlowContainer
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Height = .3f,
                                }
                            }
                        }
                    }
                }
            };
            populatePlayers();
        }

        private void populatePlayers()
        {
            foreach(var player in room.Players)
            {
                if(player != null)
                {
                    playersImages.Add(new Container
                    {
                        RelativeSizeAxes = Axes.Y,
                        Width = 180,
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
                                    Texture = textures.Get($"https://gamestogo.company/api/Users/DownloadImage/{player.BackingUser.ID}")
                                }
                            },
                            new SpriteText
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Text = player.BackingUser.Username,
                                Font = new FontUsage(size: 40)
                            },
                        }
                        }
                    });
                }
            }
        }
    }
}
