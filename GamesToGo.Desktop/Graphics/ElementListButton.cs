using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Desktop.Project;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK;

namespace GamesToGo.Desktop.Graphics
{
    public class ElementListButton: BasicButton
    {
        public ElementListButton (IProjectElement element)
        {
            RelativeSizeAxes = Axes.X;
            Height = 20;
            Children = new Drawable[]
            {
                new Box
                {
                    //Aqui debería ir la imagen predeterminada del element hasta la izquierda del boton RelativeSizeAxes = Axes.Y, Width = 20 (La que aparece por defecto cuando el usuario no ha modificado la imagen)
                },
                new SpriteText
                {
                    Text = element.Name,
                    Origin = Anchor.CentreLeft,
                    Anchor = Anchor.CentreLeft,
                    Position = new Vector2(25, 0)
                }
            };
        }
    }
}
