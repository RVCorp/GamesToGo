using System.IO;
using GamesToGo.Editor.Overlays;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osuTK;

namespace GamesToGo.Editor.Graphics
{
    public class DirectoryButton : ImageOverlayButton
    {
        private readonly IconUsage icon;
        private readonly string target;
        public DirectoryButton(string directory, DirectoryType type)
        {
            target = directory;

            switch (type)
            {
                case DirectoryType.Directory:
                    icon = FontAwesome.Solid.Folder;
                    Name = Path.GetFileName(directory);
                    break;
                case DirectoryType.Drive:
                    icon = FontAwesome.Solid.Hdd;
                    break;
                case DirectoryType.ParentDirectory:
                    icon = FontAwesome.Solid.Reply;
                    if (directory == string.Empty)
                        Name = @"Este Equipo";
                    else if(!directory.EndsWith('\\'))
                        Name = Path.GetFileName(directory);
                    break;
            }
        }

        [BackgroundDependencyLoader]
        private void load(ImageFinderOverlay imageFinder)
        {
            Height = 50;
            Action = () => imageFinder.ChangeToDirectory(target);
            AddRange(new Drawable[]
            {
                new SpriteIcon
                {
                    Icon = icon,
                    Colour = Colour4.Black,
                    Size = new Vector2(35),
                    X = 10,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                },
                new SpriteText
                {
                    X = 50,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Truncate = true,
                    MaxWidth = ImageFinderOverlay.ENTRY_WIDTH - 50 - 10,
                    Colour = Colour4.Black,
                    Text = Name,
                    Font = new FontUsage(size: 30),
                },
            });
        }
    }

    public enum DirectoryType
    {
        Directory,
        Drive,
        ParentDirectory,
    }
}
