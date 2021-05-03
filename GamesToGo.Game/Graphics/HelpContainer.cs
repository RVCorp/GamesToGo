using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Threading;
using osuTK;

namespace GamesToGo.Game.Graphics
{
    class HelpContainer : VisibilityContainer
    {
        private readonly float minHeight;
        public string Text { get; set; }
        public Vector2 Pos { get; set; }
        private Box backgroundBox;
        private TextFlowContainer textFlow;        
        private readonly BindableFloat targetHeight;

        public HelpContainer(float minHeight)
        {
            this.minHeight = minHeight;
            targetHeight = new BindableFloat(minHeight);
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Anchor = Anchor.TopCentre;
            Origin = Anchor.Centre;
            Width = 275;
            Child = new Container
            {
                RelativePositionAxes = Axes.Both,
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    backgroundBox = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Colour4.Black.Opacity(0.8f)
                    },
                    textFlow = new TextFlowContainer(font => font.Font = new FontUsage(size: 35))
                    {
                        AutoSizeAxes = Axes.Y,
                        RelativeSizeAxes = Axes.X,
                        RelativePositionAxes = Axes.Y,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Padding = new MarginPadding(12),
                    },
                },
            };
            targetHeight.BindValueChanged(e => this.ResizeHeightTo(e.NewValue, 200, Easing.OutCubic), true);
        }

        protected override void PopIn()
        {            
            Position = Pos;            
            textFlow.Text = Text;
            this.FadeIn(250);
        }

        protected override void PopOut()
        {            
            this.FadeOut(250);
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            targetHeight.Value = Math.Max(minHeight, textFlow.Height);
        }
    }
}
