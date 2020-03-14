using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK.Graphics;
using osuTK;
using GamesToGo.Desktop.Database.Models;
using osu.Framework.Allocation;
using GamesToGo.Desktop.Project;

namespace GamesToGo.Desktop.Graphics
{
    public class ProjectDescriptionButton : Button
    {
        private SpriteText projectName;
        private readonly SpriteIcon icon;
        public ProjectDescriptionButton(ProjectInfo project)
        {
            Masking = true;
            BorderThickness = 3;
            RelativeSizeAxes = Axes.X;
            Height = 80;
            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black
                },
                projectName = new SpriteText
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Position = new Vector2(15,5),
                    Colour = Color4.White,
                    Text = project.Name,
                    RelativeSizeAxes = Axes.Both
                },
                icon = new SpriteIcon
                {
                    Icon = FontAwesome.Regular.Meh,
                    RelativeSizeAxes = Axes.Y,
                    Width = 40,
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(Context database)
        {

        }
    }
}
