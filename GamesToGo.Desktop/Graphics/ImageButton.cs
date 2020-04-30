using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GamesToGo.Desktop.Overlays;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osuTK;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Graphics
{
    [LongRunningLoad]
    public class ImageButton : ImageOverlayButton
    {
        public Texture Texture { get; private set; }
        private readonly Container displayContainer;
        private string path;

        public ImageButton(string path)
        {
            this.path = path;

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
                Colour = Color4.Black,
            });

            Add(displayContainer = new Container
            {
                RelativeSizeAxes = Axes.Both,
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                Padding = new MarginPadding(15) { Bottom = 55 },
            });
        }

        [BackgroundDependencyLoader]
        private void load(ImageFinderOverlay imageFinder)
        {
            try
            {
                Texture = Texture.FromStream(File.OpenRead(path));
                Action = imageFinder.Hide;
                displayContainer.Add(new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    FillMode = FillMode.Fit,
                    Texture = Texture,
                });
            }
            catch
            {
                Texture = null;
                Action = () => imageFinder.ShowError($"Imagen demasiado grande: {path}");
                displayContainer.Add(new SpriteIcon
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Size = new Vector2(50),
                    Icon = FontAwesome.Solid.File,
                    Colour = Color4.Black,
                });
            }
        }
    }
}
