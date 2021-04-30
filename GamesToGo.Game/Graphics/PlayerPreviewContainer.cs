using System.Linq;
using GamesToGo.Common.Online;
using GamesToGo.Game.Online.Models.RequestModel;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace GamesToGo.Game.Graphics
{
    [Cached]
    public class PlayerPreviewContainer : FillFlowContainer<PlayerPreview>
    {
        [Resolved]
        private Bindable<OnlineRoom> room { get; set; }

        private readonly Bindable<Player> currentSelectedPlayer = new Bindable<Player>();
        public IBindable<Player> CurrentSelectedPlayer => currentSelectedPlayer;

        [Resolved]
        private APIController api { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Y;
            AutoSizeAxes = Axes.X;
            Direction = FillDirection.Horizontal;
            room.BindValueChanged(r => updatePreviews(), true);
        }

        public void SelectPlayer(Player player)
        {
            currentSelectedPlayer.Value = Equals(currentSelectedPlayer.Value, player) ? null : player;
        }

        private void updatePreviews()
        {
            var leftPreviews = Children.Where(i => room.Value.Players.All(p => i.Model.BackingUser.ID != p?.BackingUser.ID )).ToArray();

            var movedPreviews = Children.Except(leftPreviews)
                .Where(i =>
                    room.Value.Players.Any(p =>
                        i.Model.BackingUser.ID == p?.BackingUser.ID &&
                        i.Model.RoomPosition != p.RoomPosition))
                .Select<PlayerPreview, (int NewPosition, PlayerPreview Preview)>(i =>
                    (room.Value.Players.Single(p => p.BackingUser.ID == i.Model.BackingUser.ID).RoomPosition, i)
                );

            var newPlayers = room.Value.Players.Where(p => p != null &&
                                                           p.BackingUser.ID != api.LocalUser.Value.ID &&
                                                           Children.All(i => i.Model.BackingUser.ID != p.BackingUser.ID));

            foreach (var preview in leftPreviews)
            {
                Remove(preview);
            }

            foreach (var preview in movedPreviews)
            {
                SetLayoutPosition(preview.Preview, preview.NewPosition);
            }

            foreach (var player in newPlayers)
            {
                Insert(player.RoomPosition, new PlayerPreview(player));
            }
        }
    }
}
