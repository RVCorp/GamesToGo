using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Game.Online;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace GamesToGo.Game.Graphics
{
    public class InvitePreviewContainer : Container
    {
        public Invitation Invitation;
        private Sprite userImage;
        public Action NextScreen { get; set; }
        public Action DeleteInvitation { get; set; }

        public InvitePreviewContainer(Invitation invitation)
        {
            this.Invitation = invitation;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            RelativeSizeAxes = Axes.X;
            Height = 150;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.AliceBlue,
                },
                new FillFlowContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Direction = FillDirection.Horizontal,
                    Padding = new MarginPadding(20),
                    Spacing = new Vector2(10),
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Width = .15f,
                            Child = userImage =  new Sprite
                            {
                                RelativeSizeAxes = Axes.Both,
                                Texture = textures.Get("Images/gtg"),
                            },
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Width = .45f,
                            Child = new TextFlowContainer((w) => w.Font = new FontUsage(size: 40))
                            {
                                RelativeSizeAxes = Axes.X,
                                Height = 150,
                                Text = Invitation.Sender.Username + " te ha invitado a jugar " + Invitation.Room.Game.Name,
                                Colour = Colour4.Black,
                            }
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Width = .17f,
                            Child = new SurfaceButton
                            {                                
                                Action = () => NextScreen(),
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = new Colour4(106, 100, 104, 255)
                                    },
                                    new SpriteIcon
                                    {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        RelativeSizeAxes = Axes.Both,
                                        Icon = FontAwesome.Solid.Check
                                    }
                                }
                            }
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Width = .2f,
                            Padding = new MarginPadding(){ Right = 30 },
                            Child = new SurfaceButton
                            {
                                Action = () => DeleteInvitation(),
                                Children = new Drawable[]
                                {
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = new Colour4(106, 100, 104, 255)
                                    },
                                    new SpriteIcon
                                    {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre, 
                                        RelativeSizeAxes = Axes.Both,
                                        Icon = FontAwesome.Solid.Times
                                    }
                                }
                            }
                        }
                    }
                }
            };
            Schedule(async () =>
            {
                userImage.Texture = await textures.GetAsync($"https://gamestogo.company/api/Users/DownloadImage/{Invitation.Sender.ID}");
                if(userImage.Texture == null)
                {
                    userImage.Texture = await textures.GetAsync("Images/gtg");
                }
            });
        }
    }
}
