using System.Collections.Generic;
using GamesToGo.Game.Graphics;
using GamesToGo.Game.Online.Models.OnlineProjectElements;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace GamesToGo.Game.Overlays
{
    public class SelectionOverlay : OverlayContainer
    {
        private FillFlowContainer<CardContainer> cardsContainer;


        [BackgroundDependencyLoader]
        private void load()
        {
            Hide();
            RelativeSizeAxes = Axes.Both;
            Child = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.Black.Opacity(0.6f)
                    },
                    new Container
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Height = .3f,
                        Child = new BasicScrollContainer(Direction.Horizontal)
                        {
                            RelativeSizeAxes = Axes.Both,
                            Width = .7f,
                            ScrollbarOverlapsContent = false,
                            Child = cardsContainer = new FillFlowContainer<CardContainer>
                            {
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                RelativeSizeAxes = Axes.Y,
                                AutoSizeAxes = Axes.X,
                                Direction = FillDirection.Horizontal,
                            },
                        },
                    }
                }
            };
        }



        public void PopulateCards(List<OnlineCard> onlineCards)
        {            
            foreach(var card in onlineCards)
            {
                cardsContainer.Add(new CardContainer { Model = card });
            }
            Show();
        }

        protected override void PopIn()
        {
            this.FadeIn(300);
        }

        protected override void PopOut()
        {
            cardsContainer.Clear();
            this.FadeOut(300);
        }
    }
}
