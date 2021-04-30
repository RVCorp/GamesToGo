using System.Linq;
using GamesToGo.Game.LocalGame;
using GamesToGo.Game.LocalGame.Elements;
using GamesToGo.Game.Online.Models.OnlineProjectElements;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace GamesToGo.Game.Graphics
{
    public class TokenContainer : Container
    {
        private Token token;
        private OnlineToken model;
        private Container borderContainer;
        private ContainedImage tokenImage;

        [Resolved]
        private WorkingGame game { get; set; }

        public TokenContainer(OnlineToken token)
        {
            model = token;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            token = game.GameTokens.First(t => t.TypeID == model.TypeID);
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
