using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;
using osuTK;
using System;
using osu.Framework.Allocation;
using osu.Framework.Platform;
using osu.Framework.Graphics.Textures;
using GamesToGo.Desktop.Online;

namespace GamesToGo.Desktop.Graphics
{
    public class PublishedProjectSummaryContainer : ProjectSummaryContainer
    {
        private APIController api;
        private Storage store;
        public int ID => onlineProject.Id;

        private OnlineProject onlineProject;

        public Action<OnlineProject> ImportAction { private get; set; }

        public PublishedProjectSummaryContainer(OnlineProject onlineProject)
        {
            this.onlineProject = onlineProject;
        }

        [BackgroundDependencyLoader]
        private void load(LargeTextureStore textures, APIController api, Storage store)
        {
            this.api = api;
            this.store = store;


            BottomContainer.Add(new SpriteText
            {
                Font = new FontUsage(size: SMALL_TEXT_SIZE),
                Text = "Este juego ya fue publicado!",
            });


            var userRequest = new GetUserRequest(onlineProject.CreatorId);
            userRequest.Success += user => UsernameBox.Text = $"De {user.Username} (Ultima vez editado {onlineProject.DateTimeLastEdited:dd/MM/yyyy HH:mm})";
            api.Queue(userRequest);
        }
    }
}
