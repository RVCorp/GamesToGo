using System;
using GamesToGo.Common.Online;
using GamesToGo.Common.Online.RequestModel;
using GamesToGo.Common.Online.Requests;
using osu.Framework.Allocation;
using osu.Framework.Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osuTK;

namespace GamesToGo.Editor.Graphics
{
    [LongRunningLoad]
    public class OnlineProjectSummaryContainer : ProjectSummaryContainer
    {
        [Resolved]
        private APIController api { get; set; }

        [Resolved]
        private Storage store { get; set; }
        public int ID => onlineProject.Id;

        private SpriteIcon loadingIcon;
        private readonly OnlineGame onlineProject;
        private IconButton editButton;

        public Action<OnlineGame> ImportAction { private get; set; }

        public OnlineProjectSummaryContainer(OnlineGame onlineProject)
        {
            this.onlineProject = onlineProject;
        }

        [BackgroundDependencyLoader]
        private void load(TextureStore textures)
        {
            ButtonFlowContainer.Add(editButton = new IconButton(FontAwesome.Solid.Download, Colour4.SkyBlue, true, Colour4.PowderBlue));

            BottomContainer.Add(new SpriteText
            {
                Font = new FontUsage(size: SMALL_TEXT_SIZE),
                Text = @"Este proyecto está en el servidor, descargalo para editarlo!",
            });

            ImageContainer.Add(loadingIcon = new SpriteIcon
            {
                Size = new Vector2(.7f),
                RelativeSizeAxes = Axes.Both,
                FillMode = FillMode.Fit,
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Icon = FontAwesome.Solid.Spinner,
            });

            editButton.Enabled.Value = false;
            loadingIcon.RotateTo(0).Then().RotateTo(360, 1500).Loop();

            ProjectName.Text = onlineProject.Name;
            ProjectDescription.Text = onlineProject.Description;
            editButton.Enabled.Value = true;
            Schedule(async () => //ToDo: ????
            {
                var texture = await textures.GetAsync(@$"https://gamestogo.company/api/Games/DownloadFile/{onlineProject.Image}");

                Schedule(() =>
                {
                    ProjectImage.Texture = texture;
                    loadingIcon.FadeOut();
                });
            });

            editButton.Action += downloadProject;

            var userRequest = new GetUserRequest(onlineProject.Creator.ID);
            userRequest.Success += user => UsernameBox.Text = @$"De {user.Username} (Ultima vez editado {onlineProject.DateTimeLastEdited:dd/MM/yyyy HH:mm}) Estado: {onlineProject.Status.GetDescription()}";
            api.Queue(userRequest);
        }

        private void downloadProject()
        {
            var getGame = new DownloadProjectRequest(onlineProject.Id, onlineProject.Hash, store);
            getGame.Success += game => ImportAction?.Invoke(onlineProject);
            getGame.Progressed += progress => editButton.Progress = progress;
            api.Queue(getGame);
        }
    }
}
