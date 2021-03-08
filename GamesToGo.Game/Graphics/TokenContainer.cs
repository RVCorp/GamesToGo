using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GamesToGo.Game.LocalGame.Elements;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace GamesToGo.Game.Graphics
{
    public class TokenContainer : Container
    {
        private Token token;
        private Container borderContainer;
        private ContainedImage tokenImage;

        public TokenContainer(Token token)
        {
            this.token = token;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Anchor = Anchor.TopRight;
            Origin = Anchor.TopRight;
            RelativeSizeAxes = Axes.Both;
            Height = .2f;
            Width = .2f;
            Children = new Drawable[]
            {
                borderContainer = new Container
                {
                    Masking = true,
                    CornerRadius = 10,
                    BorderThickness = 2,
                    BorderColour = Colour4.White,
                    RelativeSizeAxes = Axes.Both,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0.1f,
                    },
                },
                tokenImage = new ContainedImage(false, 0)
                {
                    RelativeSizeAxes = Axes.Both,
                    Texture = token.Images.First(),
                    ImageSize = Size
                }
            };
        }
    }
}
