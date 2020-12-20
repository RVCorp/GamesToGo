using GamesToGo.Game.Graphics;
using GamesToGo.Game.Online;
using GamesToGo.Game.Online.Models.RequestModel;
using GamesToGo.Game.Online.Requests;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace GamesToGo.Game.Overlays
{
    public class ReportOverlay : OverlayContainer
    {
        private Box shadowBox;
        private Container content;
        private GamesToGoButton report;
        private BasicTextBox reasonTextBox;
        private FillFlowContainer reportContainer;
        private SpriteText successText;
        private GamesToGoDropdown<ReportType> reportDropdown;

        [Resolved]
        private APIController api { get; set; }

        public OnlineGame Game { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                shadowBox = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.Black,
                    Alpha = 0,
                },
                content = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding(){ Left = 100, Right = 100, Top = 525 , Bottom = 525},
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = new Colour4(106, 100, 104, 255)
                        },
                        new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Direction = FillDirection.Vertical,
                            Padding = new MarginPadding() { Bottom = 20},
                            Children = new Drawable[]
                            {
                                new Container
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Height = .1f,
                                    Child = new Container
                                    {
                                        RelativeSizeAxes = Axes.X,
                                        AutoSizeAxes = Axes.Y,
                                        Child = new SimpleIconButton(FontAwesome.Solid.Times)
                                        {
                                            Anchor = Anchor.TopRight,
                                            Origin = Anchor.TopRight,
                                            Size = new Vector2(60,60),
                                            Action = () => Hide()
                                        }
                                    },
                                },
                                new Container
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    Height = .9f,
                                    Children = new Drawable[]
                                    {
                                        reportContainer = new FillFlowContainer
                                        {
                                            RelativeSizeAxes = Axes.Both,
                                            Direction = FillDirection.Vertical,
                                            Padding = new MarginPadding(20),
                                            Children = new Drawable[]
                                            {
                                                new Container
                                                {
                                                    RelativeSizeAxes = Axes.Both,
                                                    Height = .4f,
                                                    Child = new GamePreviewContainer(Game)
                                                    {
                                                        GameNameSize = 70,
                                                        MadeBySize = 40,
                                                    }
                                                },
                                                new FillFlowContainer
                                                {
                                                    Depth = 0,
                                                    RelativeSizeAxes = Axes.Both,
                                                    Height = .25f,
                                                    Children = new Drawable[]
                                                    {
                                                        new SpriteText
                                                        {
                                                            Text = "Motivo:",
                                                            Font = new FontUsage(size:60)
                                                        },
                                                        new FillFlowContainer
                                                        {
                                                            RelativeSizeAxes = Axes.Both,
                                                            Direction = FillDirection.Horizontal,
                                                            Children = new Drawable[]
                                                            {
                                                                new Container
                                                                {
                                                                    RelativeSizeAxes = Axes.Both,
                                                                    Width = .5f,
                                                                    Children = new Drawable[]
                                                                    {
                                                                        reasonTextBox = new BasicTextBox
                                                                        {
                                                                            Height = 100,
                                                                            RelativeSizeAxes = Axes.X
                                                                        }
                                                                    }
                                                                },
                                                                new Container
                                                                {
                                                                    RelativeSizeAxes = Axes.Both,
                                                                    Width = .5f,
                                                                    Child = reportDropdown = new ReportTypeDropdown
                                                                    {
                                                                        RelativeSizeAxes = Axes.X,
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                },
                                                new SurfaceButton
                                                {
                                                    Depth = 1,
                                                    Height = .2f,
                                                    Action = () => reportAction(),
                                                    Children = new Drawable[]
                                                    {
                                                        new Box
                                                        {
                                                            RelativeSizeAxes = Axes.Both,
                                                            Colour = Colour4.LightPink
                                                        },
                                                        new SpriteText
                                                        {
                                                            Anchor = Anchor.Centre,
                                                            Origin = Anchor.Centre,
                                                            Text = "Reportar!",
                                                            Font = new FontUsage(size: 60)
                                                        }
                                                    }
                                                }
                                            }
                                        },
                                        successText = new SpriteText
                                        {
                                            Anchor = Anchor.Centre,
                                            Origin = Anchor.Centre,
                                            Font = new FontUsage(size: 60)
                                        }
                                    }
                                }
                            }
                        },                        
                    },
                },
            };
            var dropdown = new AvailableReportsRequest();
            dropdown.Success += u =>
            {
                reportDropdown.Items = u;
            };
            api.Queue(dropdown);
        }

        private void reportAction()
        {
            if(reasonTextBox.Text != "")
            {
                var reportGame = new ReportGameRequest(reasonTextBox.Text, Game.Id, reportDropdown.Current.Value.ID);
                reportGame.Success += u =>
                {
                    reportContainer.Hide();
                    successText.Text = "Se ha reportado el juego";
                };
                api.Queue(reportGame);
            }
        }

        protected override void PopIn()
        {
            shadowBox.FadeTo(0.9f, 250);
            content.FadeIn(250);
        }

        protected override void PopOut()
        {
            shadowBox.FadeOut(250, Easing.OutExpo);
            content.FadeOut(250);
        }
    }
}
