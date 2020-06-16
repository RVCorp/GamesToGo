using System;
using System.IO;
using System.Text;
using GamesToGo.Desktop.Database.Models;
using GamesToGo.Desktop.Project;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using DatabaseFile = GamesToGo.Desktop.Database.Models.File;

namespace GamesToGo.Desktop.Screens
{
    public class ProjectFileScreen : Screen
    {
        private Storage store;
        private Context database;
        private WorkingProject project;

        [BackgroundDependencyLoader]
        private void load(WorkingProject project, Context database, Storage store, ProjectEditor editor)
        {
            this.store = store;
            this.database = database;
            this.project = project;
            InternalChildren = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = new Color4 (106,100,104, 255)
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
                        new BasicButton
                        {
                            RelativeSizeAxes = Axes.Y,
                            Width = 650,
                            BackgroundColour = Color4.DarkGreen,
                            Text = "Quieres compartir tu juego con la comunidad? Publica tu juego"
                        },
                        new BasicButton
                        {
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            Width = 650,
                            Height = 200,
                            BackgroundColour = Color4.DodgerBlue,
                            Text = "Incompleto? Guarda y termina después",
                            Action = editor.SaveProject,
                        },
                        new BasicButton
                        {
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            Position = new Vector2(0, 225),
                            Width = 650,
                            Height = 200,
                            BackgroundColour = Color4.Linen,
                            Text = "Quieres un respaldo? Sube tu juego"
                        },
                        new BasicButton
                        {
                            Anchor = Anchor.BottomRight,
                            Origin = Anchor.BottomRight,
                            Width = 650,
                            Height = 200,
                            BackgroundColour = Color4.DarkSalmon,
                            Text = "Listo para publicar? Primero prueba tu juego"
                        }
                    }
                }
            };
        }
    }
}
