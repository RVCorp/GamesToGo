using GamesToGo.Desktop.Database.Models;
using GamesToGo.Desktop.Graphics;
using GamesToGo.Desktop.Project;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Screens
{
    public class ProjectFileScreen : Screen
    {
        [BackgroundDependencyLoader]
        private void load(ProjectEditor editor)
        {
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Colour4 (106,100,104, 255)
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Padding = new MarginPadding
                    {
                        Top = 200f,
                        Left = 300f,
                        Right = 300f,
                        Bottom = 200f
                    },
                    Children = new Drawable[]
                    {
                        new GamesToGoButton
                        {
                            RelativeSizeAxes = Axes.Y,
                            Width = 650,
                            BackgroundColour = Colour4.DarkGreen,
                            Text = "Quieres compartir tu juego con la comunidad? Publica tu juego"
                        },
                        new GamesToGoButton
                        {
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            Width = 650,
                            RelativeSizeAxes = Axes.Y,
                            Padding = new MarginPadding() { Bottom = 10 },
                            Height = 1/3f,
                            BackgroundColour = Colour4.DodgerBlue,
                            Text = "Incompleto? Guarda y termina después",
                            Action = () => editor.SaveProject(),
                        },
                        new GamesToGoButton
                        {
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            RelativeSizeAxes = Axes.Y,
                            Padding = new MarginPadding() { Vertical = 5 },
                            Height = 1/3f,
                            Width = 650,
                            BackgroundColour = Colour4.PaleVioletRed,
                            Text = "Quieres un respaldo? Sube tu juego",
                            Action = editor.UploadProject,
                        },
                        new GamesToGoButton
                        {
                            Anchor = Anchor.BottomRight,
                            Origin = Anchor.BottomRight,
                            Width = 650,
                            RelativeSizeAxes = Axes.Y,
                            Padding = new MarginPadding() { Top = 10 },
                            Height = 1/3f,
                            BackgroundColour = Colour4.DarkSalmon,
                            Text = "Listo para publicar? Primero prueba tu juego"
                        }
                    }
                }
            };
        }
    }
}
