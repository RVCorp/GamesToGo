using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Desktop.Project.Events;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Graphics
{
    public class ProjectEventContainer : Container
    {
        public readonly ProjectEvent Event;
        public ProjectEventContainer(ProjectEvent projectEvent)
        {
            Event = projectEvent;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            RelativeSizeAxes = Axes.X;
            Height = 80;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.SlateGray,
                },
                new SpriteText
                {
                    Margin = new MarginPadding(2) { Left = 5 },
                    Text = Event.Name.Value,
                    Font = new FontUsage(size: 35),
                },
                new SpriteText //ToDo: fijate que esto, como que no se que iria aqui
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    Margin = new MarginPadding { Left = 5, Right = 2, Top = 2, Bottom = 7},
                    Text = @"Aquí debería haber información del evento, pero soy demasiado lento como para poder hacer eso",
                    Font = new FontUsage(size: 20),
                },
            };
        }

        
    }
}
