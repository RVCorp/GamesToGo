using osu.Framework.Graphics.Sprites;
using osu.Framework.Allocation;
using osu.Framework.Platform;
using osu.Framework.Graphics.Textures;
using GamesToGo.Desktop.Online;

namespace GamesToGo.Desktop.Graphics
{
    public class PublishedProjectSummaryContainer : ProjectSummaryContainer
    {
        public int ID => onlineProject.Id;

        private readonly OnlineProject onlineProject;

        public PublishedProjectSummaryContainer(OnlineProject onlineProject)
        {
            this.onlineProject = onlineProject;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures, APIController api, Storage store)
        {
            BottomContainer.Add(new SpriteText
            {
                Font = new FontUsage(size: SMALL_TEXT_SIZE),
                Text = @"Este juego ya fue publicado!",
            });


            var userRequest = new GetUserRequest(onlineProject.CreatorId);
            userRequest.Success += user => UsernameBox.Text = @$"De {user.Username} (Ultima vez editado {onlineProject.DateTimeLastEdited:dd/MM/yyyy HH:mm})";
            api.Queue(userRequest);
        }
    }
}
