using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;
using osuTK;
using GamesToGo.Desktop.Project;
using osu.Framework.Input.Events;
using osu.Framework.Graphics.Containers;
using System;
using osu.Framework.Allocation;
using osu.Framework.Platform;
using osu.Framework.Graphics.Textures;
using GamesToGo.Desktop.Online;

namespace GamesToGo.Desktop.Graphics
{
    public class OnlineProjectSummaryContainer : Container
    {
        private const float margin_size = 5;
        private const float main_text_size = 35;
        private const float small_text_size = 20;
        private const float small_height = main_text_size + small_text_size + margin_size * 3;
        private const float expanded_height = main_text_size + small_text_size * 2 + margin_size * 4;
        private Container buttonsContainer;
        private IconButton editButton;
        private APIController api;
        private Storage store;
        public readonly int ID;


        private IconUsage editIcon;
        private Sprite projectImage;
        private SpriteText usernameBox;
        private SpriteText projectName;
        private SpriteIcon loadingIcon;
        private OnlineProject onlineProject;

        public Action<OnlineProject> ImportAction { private get; set; }
        public Action<ProjectInfo> DeleteAction { private get; set; }

        public OnlineProjectSummaryContainer(int id)
        {
            ID = id;
        }

        [BackgroundDependencyLoader]
        private void load(LargeTextureStore textures, APIController api, Storage store)
        {
            this.api = api;
            this.store = store;

            editIcon = FontAwesome.Solid.Download;

            Masking = true;
            CornerRadius = margin_size;
            BorderColour = Color4.DarkGray;
            BorderThickness = 3;
            RelativeSizeAxes = Axes.X;
            Height = small_height;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Color4(55, 55, 55, 255),
                    Alpha = 0.8f,
                },
                new Container
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Padding = new MarginPadding(margin_size),
                    Children = new Drawable[]
                    {
                        new FillFlowContainer
                        {
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(margin_size),
                            AutoSizeAxes = Axes.Both,
                            Children = new Drawable[]
                            {
                                new FillFlowContainer
                                {
                                    Direction = FillDirection.Horizontal,
                                    Spacing = new Vector2(margin_size),
                                    AutoSizeAxes = Axes.Both,
                                    Children = new Drawable[]
                                    {
                                        new Container
                                        {
                                            Size = new Vector2(main_text_size + small_text_size + margin_size),
                                            Masking = true,
                                            CornerRadius = 20 * (main_text_size + small_text_size + margin_size) / 150,
                                            Children = new Drawable[]
                                            {
                                                projectImage = new Sprite
                                                {
                                                    RelativeSizeAxes = Axes.Both,
                                                    FillMode = FillMode.Fit,
                                                    Anchor = Anchor.Centre,
                                                    Origin = Anchor.Centre,
                                                },
                                                loadingIcon = new SpriteIcon
                                                {
                                                    Size = new Vector2(.7f),
                                                    RelativeSizeAxes = Axes.Both,
                                                    FillMode = FillMode.Fit,
                                                    Anchor = Anchor.Centre,
                                                    Origin = Anchor.Centre,
                                                    Icon = FontAwesome.Solid.Spinner
                                                }
                                            }
                                        },
                                        new FillFlowContainer
                                        {
                                            Direction = FillDirection.Vertical,
                                            Spacing = new Vector2(margin_size),
                                            AutoSizeAxes = Axes.Both,
                                            Children = new[]
                                            {
                                                projectName = new SpriteText
                                                {
                                                    Font = new FontUsage(size: main_text_size),
                                                },
                                                usernameBox = new SpriteText
                                                {
                                                    Font = new FontUsage(size: small_text_size),
                                                },
                                            }
                                        }
                                    }
                                },
                                new SpriteText
                                {
                                    Font = new FontUsage(size: small_text_size),
                                    Text = "Este proyecto está en el servidor, descargalo para editarlo!",
                                }
                            }
                        }
                    },
                },
                buttonsContainer = new Container
                {
                    Padding = new MarginPadding(margin_size),
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    AutoSizeAxes = Axes.Both,
                    Alpha = 0,
                    Children = new Drawable[]
                    {
                        new FillFlowContainer<IconButton>
                        {
                            AutoSizeAxes = Axes.Both,
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            Spacing = new Vector2(margin_size),
                            Direction = FillDirection.Horizontal,
                            Children = new []
                            {
                                editButton = new IconButton(true)
                                {
                                    Icon = editIcon,
                                    ButtonColour = Color4.SkyBlue,
                                    ProgressColour = Color4.PowderBlue,
                                }
                            }
                        }
                    }
                },
            };

            editButton.Enabled.Value = false;
            loadingIcon.RotateTo(0).Then().RotateTo(360,1500).Loop();
            var getProject = new GetProjectRequest(ID);

            editButton.Action += DownloadProject;
            getProject.Success += async u =>
            {
                onlineProject = u;
                usernameBox.Text = $"Ultima vez editado {u.LastEdited:dd/MM/yyyy HH:mm}";
                projectName.Text = u.Name;
                editButton.Enabled.Value = true;
                loadingImage(await textures.GetAsync($"https://gamestogo.company/api/Games/DownloadFile/{u.Image}"));
            };
            api.Queue(getProject);
        }

        private void loadingImage(Texture texture)
        {
            loadingIcon.FadeOut();
            projectImage.Texture = texture;
        }

        protected void DownloadProject()
        {
            var getGame = new DownloadProjectRequest(onlineProject.Id, onlineProject.Hash, store);
            getGame.Success += game => ImportAction?.Invoke(onlineProject);
            getGame.Progressed += progress => editButton.Progress = progress;
            api.Queue(getGame);
        }

        protected override bool OnHover(HoverEvent e)
        {
            this.ResizeHeightTo(expanded_height, 100, Easing.InQuad);
            buttonsContainer.FadeIn(100, Easing.InQuad);
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            this.ResizeHeightTo(small_height, 100, Easing.InQuad);
            buttonsContainer.FadeOut(100, Easing.InQuad);
            base.OnHoverLost(e);
        }

        private class StatText : FillFlowContainer
        {
            public StatText(IconUsage icon, string text)
            {
                Direction = FillDirection.Horizontal;
                Spacing = new Vector2(margin_size);
                AutoSizeAxes = Axes.Both;
                Children = new Drawable[]
                {
                    new SpriteIcon
                    {
                        Size = new Vector2(small_text_size),
                        Icon = icon
                    },
                    new SpriteText
                    {
                        Font = new FontUsage(size: small_text_size),
                        Text = text,
                    },
                    new Container
                    {
                        RelativeSizeAxes = Axes.Y,
                        Width = margin_size,
                    },
                };
            }

            public StatText(IconUsage icon, int count) : this(icon, count.ToString()) { }
        }
    }
}
