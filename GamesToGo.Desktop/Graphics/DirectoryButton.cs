using GamesToGo.Desktop.Overlays;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osuTK.Graphics;
using osuTK;
using osu.Framework.Allocation;
using System.IO;

namespace GamesToGo.Desktop.Graphics
{
    public class DirectoryButton : ImageOverlayButton
    {
        private IconUsage icon;
        private string target;
        public DirectoryButton(string directory, DirectoryType type)
        {
            target = directory;

            Height = 50;
            Name = directory;

            switch (type)
            {
                case DirectoryType.Directory:
                    icon = FontAwesome.Solid.Folder;
                    directory = Path.GetFileName(directory);
                    break;
                case DirectoryType.Drive:
                    icon = FontAwesome.Solid.Hdd;
                    break;
                case DirectoryType.ParentDirectory:
                    icon = FontAwesome.Solid.Reply;
                    if (directory == string.Empty)
                        directory = "Este Equipo";
                    else if(!directory.EndsWith('\\'))
                        directory = Path.GetFileName(directory);
                    break;
            }

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
                    Text = directory,
                    Font = new FontUsage(size: 30),
                }
            });
        }

        [BackgroundDependencyLoader]
        private void load(ImageFinderOverlay imageFinder)
        {
            Action = () => imageFinder.ChangeToDirectory(target);
        }
    }

    public enum DirectoryType
    {
        Directory,
        Drive,
        ParentDirectory,
    }
}
