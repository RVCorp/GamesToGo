using GamesToGo.Game.Online.Models.RequestModel;
using GamesToGo.Game.Screens;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace GamesToGo.Game.Graphics
{
    public class PlayerPreview : Button
    {
        public readonly Player Model;
        private readonly IBindable<Player> currentSelected = new Bindable<Player>();
        private CircularContainer borderContainer;
        [Resolved]
        private GameScreen gameScreen { get; set; }

        private bool selected => (currentSelected.Value?.BackingUser.ID ?? -1) == Model.BackingUser.ID;

        [Resolved]
        private PlayerPreviewContainer playersContainer { get; set; }

        [Resolved]
        private TextureStore textures { get; set; }

        public PlayerPreview(Player model)
        {
            Model = model;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Enabled.BindTo(gameScreen.EnablePlayerSelection);
            Enabled.Value = false;
            Action += () => playersContainer.SelectPlayer(Model);
            
                      
            currentSelected.BindTo(playersContainer.CurrentSelectedPlayer);
            RelativeSizeAxes = Axes.Y;
            Width = 180;

            Child = new FillFlowContainer
            {
                RelativeSizeAxes = Axes.Both,
                Direction = FillDirection.Vertical,
                Children = new Drawable[]
                {
                    borderContainer = new CircularContainer
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        BorderColour = Colour4.White,
                        BorderThickness = 3.5f,
                        Masking = true,
                        Size = new Vector2(100),
                        Child = new Sprite
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both,
                            Texture = textures.Get($"https://gamestogo.company/api/Users/DownloadImage/{Model.BackingUser.ID}"),
                        },
                    },
                    new SpriteText
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Text = Model.BackingUser.Username,
                        Font = new FontUsage(size: 40),
                    },
                },
            };
            currentSelected.BindValueChanged(_ =>
            {
                if(Enabled.Value == true)
                    FadeBorder(selected || IsHovered, golden: selected);
            });
        }
        protected void FadeBorder(bool visible, bool instant = false, bool golden = false)
        {
            borderContainer.Colour = golden ? Colour4.Gold : Colour4.White;
        }
    }
}
