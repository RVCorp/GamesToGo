using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Editor.Graphics;
using GamesToGo.Editor.Project;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;

namespace GamesToGo.Editor.Overlays
{
    public class PreparationTurnOverlay : OverlayContainer
    {

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Colour4.MediumPurple.Opacity(0.3f),
                },
                new PreparationTurnContainer(),
            };
        }

        private class PreparationTurnContainer : TurnsContainer
        {
            [Resolved]
            private WorkingProject project { get; set; }

            [BackgroundDependencyLoader]
            private void load()
            {
                TurnActions.BindTo(project.PreparationTurn);
            }
        }

        protected override void PopIn()
        {
            this.FadeIn(250);
        }

        protected override void PopOut()
        {
            this.FadeOut(250);
        }
    }
}
