using System;
using System.IO;
using GamesToGo.Desktop.Overlays;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;
using osuTK;

namespace GamesToGo.Desktop.Graphics
{
    [LongRunningLoad]
    public class ImageButton : ImageOverlayButton
    {
        private Container displayContainer;
        private readonly string path;

        public ImageButton(string path)
        {
            this.path = path;
        }

        [BackgroundDependencyLoader]
        private void load(ImageFinderOverlay imageFinder)
        {
            Height = ImageFinderOverlay.ENTRY_WIDTH;

            Add(new SpriteText
            {
                Y = -10,
                Anchor = Anchor.BottomCentre,
                Origin = Anchor.BottomCentre,
                Font = new FontUsage(size: 30),
                Truncate = true,
                Text = Path.GetFileName(path),
                MaxWidth = ImageFinderOverlay.ENTRY_WIDTH - 20,
                Colour = Colour4.Black,
            });

            Add(displayContainer = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                Padding = new MarginPadding(15) { Bottom = 55 },
            });

            Texture tex = null;
            try
            {
                var file = File.OpenRead(path);
                tex = Texture.FromStream(file);
                file.Dispose();
            }
            catch (Exception e)
            {
                Logger.Log(@$"No se puede abrir {path}: {e.Message}", LoggingTarget.Runtime, LogLevel.Error);
            }

            if (tex == null)
            {
                Action = () => imageFinder.ShowError(@$"No se puede abrir la imagen: {path}");
                displayContainer.Add(new SpriteIcon
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(50),
                    Icon = FontAwesome.Solid.File,
                    Colour = Colour4.Black,
                });
            }
            else
            {
                Action = () => imageFinder.SelectImage(path);
                displayContainer.Add(new Sprite
                {
                    RelativeSizeAxes = displayContainer.ChildSize.X > tex.Size.X && displayContainer.ChildSize.Y > tex.Size.Y ? Axes.None : Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    FillMode = FillMode.Fit,
                    Texture = tex,
                });
            }
        }
    }
}
