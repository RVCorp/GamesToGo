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
    public class OnlineProjectSummaryContainer : ProjectSummaryContainer
    {
        private APIController api;
        private Storage store;
        public int ID => onlineProject.Id;

        private SpriteIcon loadingIcon;
        private OnlineProject onlineProject;
        private IconButton editButton;

        public Action<OnlineProject> ImportAction { private get; set; }

        public OnlineProjectSummaryContainer(OnlineProject onlineProject)
        {
            this.onlineProject = onlineProject;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures, APIController api, Storage store)
        {
            this.api = api;
            this.store = store;

            ButtonFlowContainer.Add(editButton = new IconButton(true)
            {
                Icon = FontAwesome.Solid.Download,
                ButtonColour = Colour4.SkyBlue,
                ProgressColour = Colour4.PowderBlue,
            });

            BottomContainer.Add(new SpriteText
            {
                Font = new FontUsage(size: SMALL_TEXT_SIZE),
                Text = "Este proyecto está en el servidor, descargalo para editarlo!",
            });

            ImageContainer.Add(loadingIcon = new SpriteIcon
            {
                Size = new Vector2(.7f),
                RelativeSizeAxes = Axes.Both,
                FillMode = FillMode.Fit,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Icon = FontAwesome.Solid.Spinner
            });

            editButton.Enabled.Value = false;
            loadingIcon.RotateTo(0).Then().RotateTo(360, 1500).Loop();

            ProjectName.Text = onlineProject.Name;
            ProjectDescription.Text = onlineProject.Description;
            editButton.Enabled.Value = true;
            Schedule(async () =>
            {
                ProjectImage.Texture = await textures.GetAsync($"https://gamestogo.company/api/Games/DownloadFile/{onlineProject.Image}");
                loadingIcon.FadeOut();
            });

            editButton.Action += DownloadProject;

            var userRequest = new GetUserRequest(onlineProject.CreatorId);
            userRequest.Success += user => UsernameBox.Text = $"De {user.Username} (Ultima vez editado {onlineProject.DateTimeLastEdited:dd/MM/yyyy HH:mm})";
            api.Queue(userRequest);
        }
        protected void DownloadProject()
        {
            var getGame = new DownloadProjectRequest(onlineProject.Id, onlineProject.Hash, store);
            getGame.Success += game => ImportAction?.Invoke(onlineProject);
            getGame.Progressed += progress => editButton.Progress = progress;
            api.Queue(getGame);
        }
    }
}
