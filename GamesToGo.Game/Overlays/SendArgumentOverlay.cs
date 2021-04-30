using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Game.Graphics;
using GamesToGo.Game.Screens;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace GamesToGo.Game.Overlays
{
    public class SendArgumentOverlay : OverlayContainer
    {
        [Resolved]
        private GameScreen game { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativePositionAxes = Axes.X;
            RelativeSizeAxes = Axes.Both;
            X = -1;
            InternalChildren = new Drawable[]
            {
                new CircularContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Height = .2f,
                    Width = .2f,
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Child = new SurfaceButton
                    {
                        Action = () => SendArgument(),
                        RelativeSizeAxes = Axes.Both,
                        Child = new SpriteIcon
                        {
                            RelativeSizeAxes = Axes.Both,
                            Height = .85f,
                            Width = .85f,
                            Icon = FontAwesome.Solid.PaperPlane
                        }
                    }
                }
            };
        }

        private void SendArgument ()
        {
            game.Send();
            Hide();
        }

        protected override void PopIn()
        {
            this.MoveToX(0, 300, Easing.OutBack);
        }

        protected override void PopOut()
        {
            this.MoveToX(-1, 300, Easing.InBack);
        }
    }
}
