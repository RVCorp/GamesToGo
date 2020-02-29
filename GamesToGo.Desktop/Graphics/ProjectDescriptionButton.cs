using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osuTK.Graphics;

namespace GamesToGo.Desktop.Graphics
{
    public class ProjectDescriptionButton : Button
    {
        public SpriteText ProyectName;
        public Box TextBackground;
        private readonly SpriteIcon icon;
        public ProjectDescriptionButton()
        {

            Masking = true;
            BorderThickness = 3;
            Children = new Drawable[]
            {
                TextBackground= new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour= Color4.Black
                },
                ProyectName = new SpriteText
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Position = new osuTK.Vector2(15,5),
                    Colour= Color4.White,
                    Text ="Proyecto",    //Aquí va Name de ProyectInfo.cs
                    RelativeSizeAxes = Axes.Both
                },
                icon= new SpriteIcon
                {
                    Icon = FontAwesome.Regular.Meh,
                    RelativeSizeAxes = Axes.Y,
                    Width= 40,
                    Anchor= Anchor.CentreRight,
                    Origin = Anchor.CentreRight
                }
            };
        }
    }
}
