using GamesToGo.App.Online;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace GamesToGo.App.Graphics
{
    public class RoomPreviewContainer : Container
    {
        private readonly RoomPreview room;
        private Sprite gameImage;

        public RoomPreviewContainer(RoomPreview room)
        {
            this.room = room;
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
                    Colour = Colour4.LightGray,
                },
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Direction = FillDirection.Horizontal,
                    Padding = new MarginPadding(20),
                    Spacing = new Vector2(20),
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Width = .2f,
                            Child = gameImage =  new Sprite
                            {
                                RelativeSizeAxes = Axes.Both,
                                Texture = textures.Get("Images/gtg"),
                            },
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Width = .6f,
                            Child = new FillFlowContainer
                            {
                                RelativeSizeAxes = Axes.Both,
                                Direction = FillDirection.Vertical,
                                Children = new Drawable[]
                                {
                                    new Container
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Height = .5f,
                                        Child = new TextFlowContainer((w) => w.Font = new FontUsage(size:80))
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            Text = @"Sala de " + room.Owner.Username,
                                        },
                                    },
                                    new Container
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Height = .5f,
                                        Child = new SpriteText
                                        {
                                            Text = "Id: " + room.Id,
                                            Font = new FontUsage(size:60),
                                        },
                                    },
                                },
                            },
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Width = .2f,
                            Child = new FillFlowContainer
                            {
                                RelativeSizeAxes = Axes.Both,
                                Direction = FillDirection.Horizontal,
                                Children = new Drawable[]
                                {
                                    new Container
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Width = .4f,
                                        Child = new SpriteText
                                        {
                                            Text = room.PlayersInRoom + "/" + room.Game.Maxplayers,
                                            Font = new FontUsage(size: 60)
                                        }
                                    },
                                    new Container
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Width = .6f,
                                        Padding = new MarginPadding(5),
                                        Child = new SpriteIcon
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            Icon = FontAwesome.Solid.Users
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
                gameImage.Texture = await textures.GetAsync($"https://gamestogo.company/api/Users/DownloadImage/{room.Owner.ID}");
            });
        }
    }
}
