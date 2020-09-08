using System;
using GamesToGo.Desktop.Online;
using GamesToGo.Desktop.Overlays;
using GamesToGo.Desktop.Project;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osuTK;

namespace GamesToGo.Desktop.Graphics
{
    public class LocalProjectSummaryContainer : ProjectSummaryContainer
    {
        private IconUsage editIcon;
        public ProjectInfo ProjectInfo;
        private MultipleOptionOverlay optionsOverlay;
        private APIController api;
        private WorkingProject workingProject;

        public Action<WorkingProject> EditAction { get; set; }
        public Action<ProjectInfo> DeleteAction { get; set; }

        public LocalProjectSummaryContainer(ProjectInfo projectInfo)
        {
            ProjectInfo = projectInfo;
        }

        [BackgroundDependencyLoader]
        private void load(Storage store, TextureStore textures, MultipleOptionOverlay optionsOverlay, APIController api)
        {
            this.optionsOverlay = optionsOverlay;
            this.api = api;

            workingProject = WorkingProject.Parse(ProjectInfo, store, textures, api);
            if (workingProject == null)
                editIcon = FontAwesome.Solid.ExclamationTriangle;
            else
                editIcon = FontAwesome.Solid.Edit;

            ButtonFlowContainer.AddRange(new[]
            {
                new IconButton
                {
                    Icon = FontAwesome.Solid.TrashAlt,
                    Action = showConfirmation,
                    ButtonColour = Colour4.DarkRed,
                },
                new IconButton
                {
                    Icon = editIcon,
                    Action = checkValidWorkingProject,
                    ButtonColour = workingProject == null ? FrameworkColour.YellowDark : FrameworkColour.Green,
                }
            });

            BottomContainer.Add(new FillFlowContainer
            {
                Direction = FillDirection.Horizontal,
                Spacing = new Vector2(MARGIN_SIZE),
                AutoSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    new StatText(FontAwesome.Regular.Clone, ProjectInfo.NumberCards),
                    new StatText(FontAwesome.Solid.Coins, ProjectInfo.NumberTokens),
                    new StatText(FontAwesome.Solid.ChessBoard, ProjectInfo.NumberBoards),
                    new StatText(FontAwesome.Regular.Square, ProjectInfo.NumberBoxes),
                    new StatText(FontAwesome.Solid.Users, $"{ProjectInfo.MinNumberPlayers}{(ProjectInfo.MinNumberPlayers < ProjectInfo.MaxNumberPlayers ? $"-{ProjectInfo.MaxNumberPlayers}" : "")}"),
                }
            });

            ProjectName.Text = ProjectInfo.Name;
            ProjectDescription.Text = ProjectInfo.Description;
            ProjectImage.Texture = workingProject?.Image.Value?.Texture;

            var getCreator = new GetUserRequest(ProjectInfo.CreatorID);
            getCreator.Success += u => UsernameBox.Text = $"De {u.Username} (Ultima vez editado {ProjectInfo.LastEdited:dd/MM/yyyy HH:mm})";
            api.Queue(getCreator);
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
            else if (api.LocalUser.Value.ID != ProjectInfo.CreatorID)
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
            if (api.LocalUser.Value.ID != ProjectInfo.CreatorID)
            {
                optionsOverlay.Show("Este proyecto no te pertenece, no puedes eliminarlo", new[]
                {
                    new OptionItem
                    {
                        Text = "Enterado",
                        Type = OptionType.Neutral,
                    }
                });
                return;
            }
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
        private class StatText : FillFlowContainer
        {
            public StatText(IconUsage icon, string text)
            {
                Direction = FillDirection.Horizontal;
                Spacing = new Vector2(MARGIN_SIZE);
                AutoSizeAxes = Axes.Both;
                Children = new Drawable[]
                {
                    new SpriteIcon
                    {
                        Size = new Vector2(SMALL_TEXT_SIZE),
                        Icon = icon
                    },
                    new SpriteText
                    {
                        Font = new FontUsage(size: SMALL_TEXT_SIZE),
                        Text = text,
                    },
                    new Container
                    {
                        RelativeSizeAxes = Axes.Y,
                        Width = MARGIN_SIZE,
                    },
                };
            }

            public StatText(IconUsage icon, int count) : this(icon, count.ToString()) { }
        }
    }
}
