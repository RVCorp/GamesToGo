using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Game.Graphics;
using GamesToGo.Game.Online;
using GamesToGo.Game.Online.Requests;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;

namespace GamesToGo.Game.Overlays
{
    public class InviteOverlay : OverlayContainer
    {
        [Resolved]
        private APIController api { get; set; }
        private Box shadowBox;
        private Container content;
        private FillFlowContainer<Container> onlineUsers;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                shadowBox = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Black,
                    Alpha = 0,
                },
                content = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new GridContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            RowDimensions = new[]
                            {
                                new Dimension(GridSizeMode.Relative, .2f),
                                new Dimension()
                            },
                            ColumnDimensions = new[]
                            {
                                new Dimension()
                            },
                            Content = new[]
                            {
                                new Drawable[]
                                {
                                    new Container
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Padding = new MarginPadding(10),
                                        Child = new SimpleIconButton(FontAwesome.Solid.Times)
                                        {
                                            Anchor = Anchor.TopRight,
                                            Origin = Anchor.TopRight,
                                            Action = Hide,
                                        },
                                    },
                                },
                                new Drawable[]
                                {
                                    new Container
                                    {
                                        RelativeSizeAxes = Axes.Both,
                                        Child = new BasicScrollContainer
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            ClampExtension = 30,
                                            Child = onlineUsers = new FillFlowContainer<Container>
                                            {
                                                AutoSizeAxes = Axes.Y,
                                                RelativeSizeAxes = Axes.X,
                                                Direction = FillDirection.Vertical,
                                                Padding = new MarginPadding(40)
                                            },
                                        }
                                    }
                                },
                            },
                        },
                    },
                },
            };
        }

        private void populateUsers()
        {
            onlineUsers.Clear();
            var users = new GetAllOnlineUsersRequest();
            users.Success += (u) =>
            {
                foreach(var user in u)
                {
                    onlineUsers.Add(new Container
                    {
                        RelativeSizeAxes = Axes.X,
                        Height = 200,
                        Child = new OnlineUserContainer(user)
                    });
                }
            };
            api.Queue(users);
        }

        protected override void PopIn()
        {
            populateUsers();
            shadowBox.FadeTo(0.9f, 250);
            content.FadeIn(250);
        }

        protected override void PopOut()
        {
            shadowBox.FadeOut(250, Easing.OutExpo);
            content.FadeOut(250);
        }
    }
}
