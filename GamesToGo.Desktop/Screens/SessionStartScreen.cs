using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osuTK;

namespace GamesToGo.Desktop.Screens
{
    /// <summary>
    /// Pantalla de inicio de sesión y registro de usuarios, carga un usuario y los proyectos relacionados a él. (WIP)
    /// </summary>
    public class SessionStartScreen : EmptyScreen
    {
        //Agregamos el menu principal como pantalla a la cual se deberia poder acceder
        protected override IEnumerable<Type> FollowingScreens => new[]
        {
            typeof(MainMenuScreen)
        };

        private Container cosa;
        private Vector2 speed = new Vector2(300, 300);

        public SessionStartScreen()
        {
            AddInternal(cosa = new Container
            {
                Depth = 10,
                Scale = new Vector2(2.5f, 1),
                Masking = true,
                CornerRadius = 50,
                Size = new Vector2(100, 100),
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                }
            });
        }

        protected override void Update()
        {
            base.Update();

            if (cosa.BoundingBox.Left < 0 || cosa.BoundingBox.Right > BoundingBox.Right)
                speed.X = -speed.X;
            if (cosa.BoundingBox.Top < 0 || cosa.BoundingBox.Bottom > BoundingBox.Bottom)
                speed.Y = -speed.Y;

            cosa.MoveToOffset(speed * ((float)Clock.ElapsedFrameTime / 1000f));
        }
    }
}
