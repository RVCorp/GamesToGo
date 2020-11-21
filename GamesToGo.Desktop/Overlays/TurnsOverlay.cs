using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Desktop.Graphics;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Project.Actions;
using GamesToGo.Desktop.Project.Elements;
using GamesToGo.Desktop.Project.Events;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace GamesToGo.Desktop.Overlays
{
    public class TurnsOverlay : OverlayContainer
    {
        [Resolved]
        private WorkingProject project { get; set; }
        private FillFlowContainer<ProjectEventContainer> eventsList;

        [Cached]
        private ArgumentTypeListing argumentListing = new ArgumentTypeListing();

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.Both;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Colour4 (106,100,104, 255)
                },                
                new TurnsContainer{ },
                argumentListing
            };
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
