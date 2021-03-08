using System.Collections.Generic;
using System.Linq;
using GamesToGo.Common.Online;
using GamesToGo.Game.Graphics;
using GamesToGo.Game.Online.Models.OnlineProjectElements;
using GamesToGo.Game.Online.Models.RequestModel;
using GamesToGo.Game.Online.Requests;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osu.Framework.Screens;

namespace GamesToGo.Game.Screens
{
    public class GameScreen : Screen
    {
        private PlayerHandContainer playerCards;
        [Resolved]
        private Player localPlayer { get; set; }
        private BoardsContainer board;

        [Resolved]
        private APIController api { get; set; }
        [Resolved]
        private Storage store { get; set; }
        [Resolved]
        private TextureStore textures { get; set; }

        [Resolved]
        private MainMenuScreen mainMenu { get; set; }

        [Resolved]
        private Bindable<OnlineRoom> room { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
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
                        new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Height = .1f,
                            Direction = FillDirection.Horizontal,
                            Children = new Drawable[]
                            {
                                new BasicScrollContainer(Direction.Horizontal)
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Width = .8f,
                                    ScrollbarOverlapsContent = false,
                                    Child = new PlayerPreviewContainer(),
                                },
                                new SimpleIconButton(FontAwesome.Solid.SignOutAlt)
                                {
                                    Action = exitRoom,
                                },
                            },
                        },
                        board = new BoardsContainer
                        {
                            Height = .6f,
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
                                playerCards = new PlayerHandContainer()
                            },
                        },
                    },
                },
            };
            room.BindValueChanged(updatedRoom => refreshRoom(updatedRoom.NewValue), true);
        }

        private void exitRoom()
        {
            var req = new ExitRoomRequest();
            req.Success += () =>
            {
                mainMenu.MakeCurrent();
                this.Exit();
            };
            api.Queue(req);
        }

        private void refreshRoom(OnlineRoom receivedRoom)
        {

        }
    }
}
