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
using GamesToGo.Desktop.Overlays;
using osu.Framework.Platform;
using GamesToGo.Desktop.Database.Models;
using osu.Framework.Graphics.Textures;
using GamesToGo.Desktop.Online;

namespace GamesToGo.Desktop.Graphics
{
    public class ProjectSummaryContainer : Container
    {
        private const float margin_size = 5;
        private const float main_text_size = 35;
        private const float small_text_size = 20;
        private const float small_height = main_text_size + small_text_size + margin_size * 3;
        private const float expanded_height = main_text_size + small_text_size * 2 + margin_size * 4;
        private Container buttonsContainer;
        private IconButton deleteButton;
        private IconButton editButton;
        private MultipleOptionOverlay optionsOverlay;
        private APIController api;
        public readonly ProjectInfo ProjectInfo;

        private WorkingProject workingProject;

        private IconUsage editIcon;
        private Sprite projectImage;
        private SpriteText usernameBox;

        public Action<WorkingProject> EditAction { private get; set; }
        public Action<ProjectInfo> DeleteAction { private get; set; }

        public ProjectSummaryContainer(ProjectInfo project)
        {
            ProjectInfo = project;
        }

        [BackgroundDependencyLoader]
        private void load(MultipleOptionOverlay optionsOverlay, Storage store, LargeTextureStore textures, Context database, APIController api)
        {
            this.optionsOverlay = optionsOverlay;
            this.api = api;

            workingProject = WorkingProject.Parse(ProjectInfo, store, textures, database);
            if (workingProject == null)
                editIcon = FontAwesome.Solid.ExclamationTriangle;
            else
                editIcon = FontAwesome.Solid.Edit;

            var getCreator = new GetUserRequest(workingProject.DatabaseObject.CreatorID);
            getCreator.Success += u => usernameBox.Text = $"De {u.Username} (Ultima vez editado {ProjectInfo.LastEdited:dd/MM/yyyy HH:mm})";

            api.Queue(getCreator);

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
                                            Child = projectImage = new Sprite
                                            {
                                                RelativeSizeAxes = Axes.Both,
                                                FillMode = FillMode.Fit
                                            }
                                        },
                                        new FillFlowContainer
                                        {
                                            Direction = FillDirection.Vertical,
                                            Spacing = new Vector2(margin_size),
                                            AutoSizeAxes = Axes.Both,
                                            Children = new[]
                                            {
                                                new SpriteText
                                                {
                                                    Font = new FontUsage(size: main_text_size),
                                                    Text = ProjectInfo.Name,
                                                },
                                                usernameBox = new SpriteText
                                                {
                                                    Font = new FontUsage(size: small_text_size),
                                                },
                                            }
                                        }
                                    }
                                },
                                new FillFlowContainer
                                {
                                    Direction = FillDirection.Horizontal,
                                    Spacing = new Vector2(margin_size),
                                    AutoSizeAxes = Axes.Both,
                                    Children = new Drawable[]
                                    {
                                        new StatText(FontAwesome.Regular.Clone, ProjectInfo.NumberCards),
                                        new StatText(FontAwesome.Solid.Coins, ProjectInfo.NumberTokens),
                                        new StatText(FontAwesome.Solid.ChessBoard, ProjectInfo.NumberBoards),
                                        new StatText(FontAwesome.Regular.Square, ProjectInfo.NumberBoxes),
                                        new StatText(FontAwesome.Solid.Users, $"{ProjectInfo.MinNumberPlayers}{(ProjectInfo.MinNumberPlayers < ProjectInfo.MaxNumberPlayers ? $"-{ProjectInfo.MaxNumberPlayers}" : "")}"),
                                    }
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
                                deleteButton = new IconButton
                                {
                                    Icon = FontAwesome.Solid.TrashAlt,
                                    Action = showConfirmation,
                                    ButtonColour = Color4.DarkRed,
                                },
                                editButton = new IconButton
                                {
                                    Icon = editIcon,
                                    Action = checkValidWorkingProject,
                                    ButtonColour = workingProject == null ? FrameworkColour.YellowDark : FrameworkColour.Green,
                                }
                            }
                        }
                    }
                },
            };
        }

        private void checkValidWorkingProject()
        {
            if (workingProject == null)
            {
                optionsOverlay.Show("Este proyecto no se puede abrir. ¿Qué deseas hacer con el?", new[]
                {
                    new OptionItem
                    {
                        Text = "Eliminarlo",
                        Action = showConfirmation,
                        Type = OptionType.Destructive,
                    },
                    new OptionItem
                    {
                        Text = "Buscarlo en el servidor",
                        Type = OptionType.Additive,
                    },
                    new OptionItem
                    {
                        Text = "Nada",
                        Type = OptionType.Neutral,
                    }
                });
            }
            else if(api.LocalUser.Value.ID != workingProject.DatabaseObject.CreatorID)
            {
                optionsOverlay.Show("Este proyecto no te pertenece, no puedes editarlo", new[]
                {
                    new OptionItem
                    {
                        Text = "Enterado",
                        Type = OptionType.Neutral,
                    }
                });
            }
            else
            {
                EditAction?.Invoke(workingProject);
            }
        }

        private void showConfirmation()
        {
            optionsOverlay.Show($"Seguro que quieres eliminar el proyecto \'{ProjectInfo.Name}\'", new[]
            {
                new OptionItem
                {
                    Text = "¡A la basura!",
                    Action = () => DeleteAction?.Invoke(ProjectInfo),
                    Type = OptionType.Destructive,
                },
                new OptionItem
                {
                    Text = "Mejor me lo quedo",
                    Action = () => { },
                    Type = OptionType.Neutral,
                }
            });
        }

        protected override bool OnHover(HoverEvent e)
        {
            this.ResizeHeightTo(expanded_height, 100, Easing.InQuad);
            buttonsContainer.FadeIn(100, Easing.InQuad)
                .OnComplete(_ =>
                {
                    deleteButton.Enabled.Value = true;
                    editButton.Enabled.Value = true;
                });
            return true;
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            this.ResizeHeightTo(small_height, 100, Easing.InQuad);
            buttonsContainer.FadeOut(100, Easing.InQuad)
                .OnComplete(_ =>
                {
                    deleteButton.Enabled.Value = false;
                    editButton.Enabled.Value = false;
                });
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
