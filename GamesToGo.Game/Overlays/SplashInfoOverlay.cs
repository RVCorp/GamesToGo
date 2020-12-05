using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;

namespace GamesToGo.Game.Overlays
{
    public class SplashInfoOverlay : OverlayContainer
    {
        private Box backgroundBox;
        public TextFlowContainer TextFlow;
        [Resolved]
        private GamesToGoGame game { get; set; }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.X;
            Height = 150;
            Anchor = Anchor.TopCentre;
            Origin = Anchor.BottomCentre;
            Child = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Children = new Drawable[]
                {
                    backgroundBox = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                    },

                    TextFlow = new TextFlowContainer((w) => w.Font = new FontUsage(size:60))
                    {
                        RelativeSizeAxes = Axes.Both,
                    }
                },
            };
            game.Invitations.CollectionChanged += invitationReceived;
        }

        private void invitationReceived(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch(e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                {                   
                    int n = game.Invitations.Count;
                    Show(game.Invitations[n - 1].Sender.Username + " te ha invitado a jugar", Colour4.LightBlue);
                }
                break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                {
                    
                }
                break;
                default:
                {

                }break;
            }
        }

        public void Show(string text, Colour4 color)
        {
            TextFlow.Clear();
            TextFlow.AddText(text);
            if (LatestTransformEndTime > Clock.CurrentTime)
            {
                backgroundBox.FadeColour(color, 300, Easing.OutCubic);
                TextFlow.MoveToY(1)
                    .Then()
                    .MoveToY(0, 200, Easing.OutCubic);
            }
            else
            {
                backgroundBox.Colour = color;
            }

            if (Math.Abs(Y - 150) < 0.0001f)
                ClearTransforms();

            this.MoveToY(150, 400, Easing.OutCubic)
            .Delay(4000)
            .MoveToY(0, 400, Easing.OutCubic)
            .OnComplete(_ => Hide());

            Show();
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            Hide();
            ClearTransforms();
            this.MoveToY(0, 400, Easing.OutCubic)
            .OnComplete(_ => Hide());
            return true;
        }

        protected override void PopIn()
        {

        }

        protected override void PopOut()
        {

        }
    }
}
