using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GamesToGo.Common.Online.RequestModel;
using GamesToGo.Game.LocalGame;
using GamesToGo.Game.LocalGame.Elements;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;

namespace GamesToGo.Game.Graphics
{
    public class TestContainer : Container
    {
        [Resolved]
        private WorkingGame game { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Child = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                Children = new Drawable[]
                {
                    new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Height = .1f,
                        Child = new BasicScrollContainer(Direction.Horizontal)
                        {
                            RelativeSizeAxes = Axes.Both,
                            Width = .8f,
                            ScrollbarOverlapsContent = false,
                            Child = new FillFlowContainer
                            {
                                
                            }
                        }
                    },
                    new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Height = .6f,
                        Child = new BoardsContainer()
                        {
                            Boards = game.GameBoards.ToList()
                        }
                    },
                    new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Height = .3f,
                        Child = new PlayerHandContainer()
                    }
                }
            };
        }        
    }
}
