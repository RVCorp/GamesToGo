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
        private Token fileToken;
        private OnlineToken model;
        public OnlineToken Model
        {
            get => model;
            set
            {
                model = value;

                CheckToken();
            }
        }

        [Resolved]
        private WorkingGame game { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            fileToken = game.GameTokens.First(t => t.TypeID == model.TypeID);
            Anchor = Anchor.TopRight;
            Origin = Anchor.TopRight;
            Height = 300;
            Width = 300;
            Children = new Drawable[]
            {
                new Container
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
                new ContainedImage(false, 0)
                {
                    RelativeSizeAxes = Axes.Both,
                    Texture = fileToken.Images.First(),
                    ImageSize = Size
                }
            };
        }

        public void CheckToken()    
        {
            switch(model.Privacy)
            {
                case ElementPrivacy.Invisible:
                {
                    Hide();
                }break;
                case ElementPrivacy.Private:
                {
                    Show();
                }break;
                case ElementPrivacy.Public:
                {
                    Show();
                }break;
            }
        }
    }
}
