using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osuTK;

namespace GamesToGo.Editor.Graphics
{
    public class ArgumentSelectorOverlay : OverlayContainer
    {
        protected override bool StartHidden => true;
        public Container Current;

        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => State.Value == Visibility.Visible;

        protected override bool ReceivePositionalInputAtSubTree(Vector2 screenSpacePos) => State.Value == Visibility.Visible;

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
        }

        public void Show(Container selector)
        {
            Current = selector;
            Add(Current);
            Show();
        }

        protected override bool Handle(UIEvent e)
        {
            switch (e)
            {
                case ClickEvent _:
                    Hide();
                    return false;
            }

            return true;
        }

        protected override void PopIn()
        {
            this.FadeIn();
        }

        protected override void PopOut()
        {
            this.FadeOut();

            if (Current == null)
                return;
            Remove(Current);
            Current = null;
        }
    }
}
