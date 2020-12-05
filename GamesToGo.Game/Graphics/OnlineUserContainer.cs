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
    public class OnlineUserContainer : Container
    {
        [Resolved]
        private APIController api { get; set; }
        private User user;
        private Sprite userImage;
        private SurfaceButton inviteButton;
        private Box colourBox;

        public OnlineUserContainer(User user)
        {
            this.user = user;
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
                    Spacing = new Vector2(10),
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Width = .2f,
                            Child = userImage =  new Sprite
                            {
                                RelativeSizeAxes = Axes.Both,
                                Texture = textures.Get("Images/gtg"),
                            },
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Width = .6f,
                            Child = new Container
                            {
                                RelativeSizeAxes = Axes.Both,
                                Height = .5f,
                                Child = new TextFlowContainer((w) => w.Font = new FontUsage(size:100))
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Text = user.Username
                                },
                            },
                        },
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Width = .2f,
                            Padding = new MarginPadding(){ Right = 50},
                            Child = inviteButton = new SurfaceButton
                            {
                                Action = () => inviteUser(),
                                Children = new Drawable[]
                                {
                                    colourBox = new Box
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Colour = new Colour4(106, 100, 104, 255)
                                    },
                                    new SpriteIcon
                                    {
                                        Anchor = Anchor.Centre,
                                        Origin = Anchor.Centre,
                                        RelativeSizeAxes = Axes.Both,
                                        Width = .9f,
                                        Icon = FontAwesome.Solid.Envelope
                                    }
                                }
                            },
                        },
                    },
                },
            };
            Schedule(async () =>
            {
                userImage.Texture = await textures.GetAsync($"https://gamestogo.company/api/Users/DownloadImage/{user.ID}");
            });
        }

        private void inviteUser()
        {
            inviteButton.Enabled.Value = false;
            colourBox.Colour = Colour4.DarkSlateGray;
            var invite = new SendInvitationRequest(user.ID);
            invite.Failure += e =>
            {
                inviteButton.Enabled.Value = true;
                colourBox.Colour = new Colour4(106, 100, 104, 255);
            };
            api.Queue(invite);
        }
    }
}
