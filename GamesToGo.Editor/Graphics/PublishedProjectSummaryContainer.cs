using GamesToGo.Editor.Online;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osuTK;

namespace GamesToGo.Editor.Graphics
{
    public class PublishedProjectSummaryContainer : ProjectSummaryContainer
    {
        public int ID => onlineProject.Id;

        private readonly OnlineProject onlineProject;
        private SpriteIcon loadingIcon;

        public PublishedProjectSummaryContainer(OnlineProject onlineProject)
        {
            this.onlineProject = onlineProject;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures, APIController api, Storage store)
        {
            ProjectName.Text = onlineProject.Name;
            ProjectDescription.Text = onlineProject.Description;
            ImageContainer.Add(loadingIcon = new SpriteIcon
            {
                Size = new Vector2(.7f),
                RelativeSizeAxes = Axes.Both,
                FillMode = FillMode.Fit,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Icon = FontAwesome.Solid.Spinner,
            });
            Schedule(async () => //ToDo: ????
            {
                ProjectImage.Texture = await textures.GetAsync(@$"https://gamestogo.company/api/Games/DownloadFile/{onlineProject.Image}");
                loadingIcon.FadeOut();
            });

            BottomContainer.Add(new SpriteText
            {
                Font = new FontUsage(size: SMALL_TEXT_SIZE),
                Text = @"Este juego ya fue publicado!",
            });

            var userRequest = new GetUserRequest(onlineProject.Creator.ID);
            userRequest.Success += user => UsernameBox.Text = @$"De {user.Username} (Ultima vez editado {onlineProject.DateTimeLastEdited:dd/MM/yyyy HH:mm})";
            api.Queue(userRequest);
        }
    }
}
